using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ModelResourcesDesc _mrd = new ModelResourcesDesc();
        private GdaService _gda;

        private readonly List<long> _allGids = new List<long>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Unloaded += (s, e) => _gda?.Dispose();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _gda = new GdaService();

                // 1) Popuni ExtentModelCode listu svim entity tipovima
                foreach (DMSType t in Enum.GetValues(typeof(DMSType)))
                {
                    if (t == DMSType.MASK_TYPE) continue;
                    try
                    {
                        var mc = _mrd.GetModelCodeFromType(t);
                        ExtentModelCode.Items.Add(mc);
                    }
                    catch { /* TO DO */ }
                }

                // 2) Učitaj sve GID-ove u Values i Related tabu (radi komfora)
                await LoadAllGidsForUI();

                if (ExtentModelCode.Items.Count > 0)
                    ExtentModelCode.SelectedIndex = 0;

                if (ValuesGid.Items.Count > 0)
                    ValuesGid.SelectedIndex = 0;

                if (RelatedSourceGid.Items.Count > 0)
                    RelatedSourceGid.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Init error");
            }
        }

        private async Task LoadAllGidsForUI()
        {
            _allGids.Clear();
            ValuesGid.Items.Clear();
            RelatedSourceGid.Items.Clear();

            // Za svaki entity tip, pokupi GID-ove
            foreach (DMSType t in Enum.GetValues(typeof(DMSType)))
            {
                if (t == DMSType.MASK_TYPE) 
                    continue;

                ModelCode mc;

                try 
                { 
                    mc = _mrd.GetModelCodeFromType(t); 
                }
                catch 
                { 
                    continue; 
                }

                var ids = await Task.Run(() => _gda.GetExtentIds(mc));
                foreach (var id in ids)
                {
                    _allGids.Add(id);
                    var hex = $"0x{id:X16}";
                    ValuesGid.Items.Add(hex);
                    RelatedSourceGid.Items.Add(hex);
                }
            }
        }

        // EXTENT
        private void ExtentModelCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExtentPropsPanel.Children.Clear();
            if (ExtentModelCode.SelectedItem is ModelCode mc)
            {
                var type = ModelResourcesDesc.GetTypeFromModelCode(mc);
                var props = _mrd.GetAllPropertyIds(type);

                foreach (var p in props)
                {
                    var cb = new CheckBox { Content = p.ToString(), Tag = p, Margin = new Thickness(0, 2, 0, 2) };
                    ExtentPropsPanel.Children.Add(cb);
                }
            }
        }

        private async void BtnGetExtent_Click(object sender, RoutedEventArgs e)
        {
            if (!(ExtentModelCode.SelectedItem is ModelCode mc))
                return;

            var selectedProps = GetCheckedProperties(ExtentPropsPanel);
            if (selectedProps.Count == 0)
            {
                // default minimalni set za prikaz
                selectedProps.Add(ModelCode.IDOBJ_MRID);
                selectedProps.Add(ModelCode.IDOBJ_NAME);
            }

            try
            {
                ExtentOutput.Text = "Loading...";
                var rds = await Task.Run(() => _gda.GetExtentValues(mc, selectedProps));
                ExtentOutput.Text = FormatResourceDescriptions(rds);
            }
            catch (Exception ex)
            {
                ExtentOutput.Text = ex.Message;
            }
        }

        // VALUES
        private void ValuesGid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValuesPropsPanel.Children.Clear();
            if (ValuesGid.SelectedItem is string s && TryParseGid(s, out long gid))
            {
                var t = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
                var props = _mrd.GetAllPropertyIds(t);

                foreach (var p in props)
                {
                    var cb = new CheckBox { Content = p.ToString(), Tag = p, Margin = new Thickness(0, 2, 0, 2) };
                    ValuesPropsPanel.Children.Add(cb);
                }
            }
        }

        private async void BtnGetValues_Click(object sender, RoutedEventArgs e)
        {
            if (!(ValuesGid.SelectedItem is string s) || !TryParseGid(s, out long gid))
                return;

            var selectedProps = GetCheckedProperties(ValuesPropsPanel);
            if (selectedProps.Count == 0)
            {
                selectedProps.Add(ModelCode.IDOBJ_MRID);
                selectedProps.Add(ModelCode.IDOBJ_NAME);
            }

            try
            {
                ValuesOutput.Text = "Loading...";
                var rd = await Task.Run(() => _gda.GetValues(gid, selectedProps));
                ValuesOutput.Text = FormatResourceDescriptions(new List<ResourceDescription> { rd });
            }
            catch (Exception ex)
            {
                ValuesOutput.Text = ex.Message;
            }
        }

        // RELATED
        private void RelatedSourceGid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RelatedProperty.Items.Clear();
            RelatedType.Items.Clear();
            RelatedPropsPanel.Children.Clear();

            if (!(RelatedSourceGid.SelectedItem is string s) || !TryParseGid(s, out long gid))
                return;

            var t = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
            var props = _mrd.GetAllPropertyIds(t);

            // SAMO reference / refVector propertije kao asocijaciju
            foreach (var p in props)
            {
                var prop = new Property(p);
                if (prop.Type == PropertyType.Reference || prop.Type == PropertyType.ReferenceVector)
                {
                    RelatedProperty.Items.Add(p);
                }
            }

            //Add props as cb
            foreach (var p in props)
            {
                var cb = new CheckBox { Content = p.ToString(), Tag = p, Margin = new Thickness(0, 2, 0, 2) };
                RelatedPropsPanel.Children.Add(cb);
            }

            // opcioni TYPE filter: dozvoli sve tipove
            foreach (DMSType dt in Enum.GetValues(typeof(DMSType)))
            {
                if (dt == DMSType.MASK_TYPE) continue;
                try { RelatedType.Items.Add(_mrd.GetModelCodeFromType(dt)); } catch { }
            }
        }

        private async void BtnGetRelated_Click(object sender, RoutedEventArgs e)
        {
            if (!(RelatedSourceGid.SelectedItem is string s) || !TryParseGid(s, out long source))
                return;
            if (!(RelatedProperty.SelectedItem is ModelCode assocProp))
            {
                MessageBox.Show("Odaberi association property (Reference / ReferenceVector).");
                return;
            }

            var targetType = (RelatedType.SelectedItem is ModelCode mcType) ? mcType : 0; // 0 -> bez filtera
            var selectedProps = GetCheckedProperties(RelatedPropsPanel);
            if (selectedProps.Count == 0)
            {
                selectedProps.Add(ModelCode.IDOBJ_MRID);
                selectedProps.Add(ModelCode.IDOBJ_NAME);
            }

            try
            {
                RelatedOutput.Text = "Loading...";
                var rds = await Task.Run(() => _gda.GetRelatedValues(source, assocProp, targetType, selectedProps));
                RelatedOutput.Text = FormatResourceDescriptions(rds);
            }
            catch (Exception ex)
            {
                RelatedOutput.Text = ex.Message;
            }
        }

        private List<ModelCode> GetCheckedProperties(StackPanel panel)
        {
            var list = new List<ModelCode>();
            foreach (var child in panel.Children)
            {
                if (child is CheckBox cb && cb.IsChecked == true && cb.Tag is ModelCode mc)
                    list.Add(mc);
            }
            return list;
        }

        private static bool TryParseGid(string input, out long gid)
        {
            input = input.Trim();
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                return long.TryParse(input.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out gid);
            return long.TryParse(input, out gid);
        }

        private static string FormatResourceDescriptions(IEnumerable<ResourceDescription> rds)
        {
            var sb = new StringBuilder();
            foreach (var rd in rds)
            {
                if (rd == null) continue;
                sb.AppendLine($"Entity with gid: 0x{rd.Id:X16}");
                foreach (var prop in rd.Properties)
                {
                    switch (prop.Type)
                    {
                        case PropertyType.Int64:
                            sb.AppendLine($"\t{prop.Id}: 0x{prop.AsLong():X16}");
                            break;
                        case PropertyType.Float:
                            sb.AppendLine($"\t{prop.Id}: {prop.AsFloat()}");
                            break;
                        case PropertyType.String:
                            sb.AppendLine($"\t{prop.Id}: {prop.AsString()}");
                            break;
                        case PropertyType.Reference:
                            sb.AppendLine($"\t{prop.Id}: 0x{prop.AsReference():X16}");
                            break;
                        case PropertyType.ReferenceVector:
                            sb.AppendLine($"\t{prop.Id}:");
                            foreach (var r in prop.AsReferences())
                                sb.AppendLine($"\t\t0x{r:X16}");
                            break;
                        case PropertyType.DateTime:
                            sb.AppendLine($"\t{prop.Id}: {prop.AsDateTime()}");
                            break;
                        case PropertyType.Enum:
                            sb.AppendLine($"\t{prop.Id}: {prop.AsEnum()}");
                            break;
                        case PropertyType.Bool:
                            sb.AppendLine($"\t{prop.Id}: {prop.AsBool()}");
                            break;
                        default:
                            sb.AppendLine($"\t{prop.Id}: {prop.PropertyValue.LongValue}");
                            break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}
