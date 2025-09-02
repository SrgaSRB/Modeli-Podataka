using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    public class FTN11_ProfileImporter
    {
        #region Singleton
        private static FTN11_ProfileImporter importer = null;
        private static readonly object singletoneLock = new object();

        public static FTN11_ProfileImporter Instance
        {
            get
            {
                if (importer == null)
                {
                    lock (singletoneLock)
                    {
                        if (importer == null)
                        {
                            importer = new FTN11_ProfileImporter();
                            importer.Reset();
                        }
                    }
                }
                return importer;
            }
        }
        #endregion

        private ConcreteModel concreteModel;
        private Delta delta;
        private ImportHelper importHelper;
        private TransformAndLoadReport report;

        public Delta NMSDelta => delta;

        public void Reset()
        {
            concreteModel = null;
            delta = new Delta();
            importHelper = new ImportHelper();
            report = null;
        }

        public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
        {
            LogManager.Log("Importing FTN11 profile elements.", LogLevel.Info);
            report = new TransformAndLoadReport();
            concreteModel = cimConcreteModel;
            delta.ClearDeltaOperations();

            if ((concreteModel != null) && (concreteModel.ModelMap != null))
            {
                try
                {
                    ConvertModelAndPopulateDelta();
                }
                catch (Exception ex)
                {
                    string message = $"{DateTime.Now} - ERROR in data import - {ex.Message}";
                    LogManager.Log(message);
                    report.Report.AppendLine(ex.Message);
                    report.Success = false;
                }
            }

            LogManager.Log("Importing FTN11 profile elements - END.", LogLevel.Info);
            return report;
        }

        /// <summary> Redosled zbog referenci:
        /// Curve → OutageSchedule → SwitchingOperation → GroundDisconnector → CurveData → IrregularTimePoint
        /// (CurveData -> Curve; ITP -> OutageSchedule; SO -> OutageSchedule)
        /// </summary>
        private void ConvertModelAndPopulateDelta()
        {
            LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            ImportCurves();
            ImportOutageSchedules();
            ImportSwitchingOperations();
            ImportGroundDisconnectors();
            ImportCurveDatas();
            ImportIrregularTimePoints();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import helpers (CreateRD + Add to Delta)
        private void ImportCurves()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.Curve");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.Curve cim = pair.Value as FTN.Curve;
                ResourceDescription rd = CreateCurveRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("Curve ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("Curve ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateCurveRD(FTN.Curve cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CURVE, importHelper.CheckOutIndexForDMSType(DMSType.CURVE));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateCurveProperties(cim, rd);
            return rd;
        }

        private void ImportCurveDatas()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.CurveData");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.CurveData cim = pair.Value as FTN.CurveData;
                ResourceDescription rd = CreateCurveDataRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("CurveData ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("CurveData ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateCurveDataRD(FTN.CurveData cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CURVEDATA, importHelper.CheckOutIndexForDMSType(DMSType.CURVEDATA));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateCurveDataProperties(cim, rd, importHelper, report);
            return rd;
        }

        private void ImportOutageSchedules()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.OutageSchedule");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.OutageSchedule cim = pair.Value as FTN.OutageSchedule;
                ResourceDescription rd = CreateOutageScheduleRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("OutageSchedule ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("OutageSchedule ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateOutageScheduleRD(FTN.OutageSchedule cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.OUTAGESCH, importHelper.CheckOutIndexForDMSType(DMSType.OUTAGESCH));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateOutageScheduleProperties(cim, rd);
            return rd;
        }

        private void ImportIrregularTimePoints()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.IrregularTimePoint");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.IrregularTimePoint cim = pair.Value as FTN.IrregularTimePoint;
                ResourceDescription rd = CreateIrregularTimePointRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("IrregularTimePoint ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("IrregularTimePoint ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateIrregularTimePointRD(FTN.IrregularTimePoint cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ITP, importHelper.CheckOutIndexForDMSType(DMSType.ITP));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateIrregularTimePointProperties(cim, rd, importHelper, report);
            return rd;
        }

        private void ImportSwitchingOperations()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.SwitchingOperation");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.SwitchingOperation cim = pair.Value as FTN.SwitchingOperation;
                ResourceDescription rd = CreateSwitchingOperationRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("SwitchingOperation ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("SwitchingOperation ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateSwitchingOperationRD(FTN.SwitchingOperation cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SO, importHelper.CheckOutIndexForDMSType(DMSType.SO));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateSwitchingOperationProperties(cim, rd, importHelper, report);
            return rd;
        }

        private void ImportGroundDisconnectors()
        {
            SortedDictionary<string, object> cimObjects = concreteModel.GetAllObjectsOfType("FTN.GroundDisconnector");
            if (cimObjects == null) return;

            foreach (var pair in cimObjects)
            {
                FTN.GroundDisconnector cim = pair.Value as FTN.GroundDisconnector;
                ResourceDescription rd = CreateGroundDisconnectorRD(cim);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("GroundDisconnector ID = ").Append(cim.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("GroundDisconnector ID = ").Append(cim.ID).AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateGroundDisconnectorRD(FTN.GroundDisconnector cim)
        {
            if (cim == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.GNDDISC, importHelper.CheckOutIndexForDMSType(DMSType.GNDDISC));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cim.ID, gid);

            FTN11_ProfileConverter.PopulateGroundDisconnectorProperties(cim, rd);
            return rd;
        }
        #endregion
    }
}
