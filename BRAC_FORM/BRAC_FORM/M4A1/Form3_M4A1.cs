using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen.Annotations;
using NXOpen.Drawings;
using NXOpen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BRAC_FORM
{
    public partial class Form3_M4A1 : Form
    {
        public Form3_M4A1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) ////////////// PREVIOUS
        {
            Form2_M4A1 Form2_M4A1 = new Form2_M4A1(); // Create an instance of Form1
            Form2_M4A1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button1_Click(object sender, EventArgs e) /////////////////// DRAWINGS
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            // Skapa assemblyritning
            FileNew fileNew1 = theSession.Parts.FileNew();
            fileNew1.TemplateFileName = "sts-A3.prt";
            fileNew1.UseBlankTemplate = false;
            fileNew1.ApplicationName = "DrawingTemplate";
            fileNew1.Units = Part.Units.Millimeters;
            fileNew1.UsesMasterModel = "Yes";
            fileNew1.TemplateType = FileNewTemplateType.Item;
            fileNew1.TemplatePresentationName = "STS - A3";

            string filename = GlobalVariables.FilePath + "\\" + textBox1.Text + ".prt";
            fileNew1.NewFileName = filename;


            //fileNew1.NewFileName = @"C:\\Users\\u107284\\Desktop\\REEPOE\\BRAC_FORM\\CAD\\assembly1_dwg1.prt";
            fileNew1.MasterFileName = "assembly1";


            fileNew1.MakeDisplayedPart = true;
            fileNew1.DisplayPartOption = DisplayPartOption.AllowAdditional;
            fileNew1.Commit();
            fileNew1.Destroy();


            workPart = theSession.Parts.Work;
            workPart.Drafting.EnterDraftingApplication();
            workPart.Drafting.SetTemplateInstantiationIsComplete(true);

            // === FRONT ===+++
            BaseViewBuilder frontBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            frontBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Front") as ModelingView;
            frontBuilder.Placement.Placement.SetValue(null, workPart.Views.WorkView, new Point3d(100, 100, 0));
            frontBuilder.Scale.Denominator = 3.0; ///skalan!
            BaseView frontView = (BaseView)frontBuilder.Commit();
            frontBuilder.Destroy();

            // === RIGHT ===
            BaseViewBuilder rightBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            rightBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Right") as ModelingView;
            rightBuilder.Placement.Placement.SetValue(frontView, workPart.Views.WorkView, new Point3d(220, 100, 0));
            rightBuilder.Scale.Denominator = 3.0;//skalan!
            BaseView rightView = (BaseView)rightBuilder.Commit();
            rightBuilder.Destroy();

            // === ISOMETRIC ===
            BaseViewBuilder isoBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            isoBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Isometric") as ModelingView;
            isoBuilder.Placement.Placement.SetValue(null, workPart.Views.WorkView, new Point3d(210, 200, 0));
            isoBuilder.Scale.Denominator = 3.0;//skalan!
            BaseView isoView = (BaseView)isoBuilder.Commit();
            isoBuilder.Destroy();

            // Parts list
            PlistBuilder plistBuilder = workPart.Annotations.PartsLists.CreatePartsListBuilder(null);
            plistBuilder.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
            plistBuilder.Contents.TopLevelAssembly = NXOpen.Annotations.PartsListContentsBuilder.TopLevelAssemblyType.ChildPart;
            plistBuilder.Origin.Origin.SetValue(null, null, new Point3d(300, 270, 0));
            plistBuilder.Commit();
            plistBuilder.Destroy();

            // === Skapa separata detaljritningar i nya NX-fönster ===
            HideDatumsAndSketches1();
            CreateSeparateDetailDrawing($"M6_35_NEW_BRAC_{GlobalVariables.bracketCounter}", 2);

            CreateSeparateDetailDrawing($"Upper_NEW_clamp_{GlobalVariables.bracketCounter}", 1);

            CreateSeparateDetailDrawing($"Lower_brac_new_m16_{GlobalVariables.bracketCounter}", 2);

            CreateSeparateDetailDrawing($"Locking_brack_{GlobalVariables.bracketCounter}", 2);

            CreateSeparateDetailDrawing($"RPD_PIN_{GlobalVariables.bracketCounter}", 3);

            MessageBox.Show("All drawings created.");

            //theSession.ApplicationSwitchImmediate("UG_APP_MODELING"); Denna ska fixa saker men fixar inget
        }
        private void CreateSeparateDetailDrawing(string partName, double scale)
        {
            Session theSession = Session.GetSession();
            string baseDir = GlobalVariables.FilePath + "\\";
            string partPath = baseDir + partName + ".prt";
            string drawingPath = baseDir + partName + "_dwg1.prt";

            Part detailPart = null;

            foreach (Part p in theSession.Parts.ToArray())
            {
                if (p.FullPath.ToLower() == partPath.ToLower())
                {
                    detailPart = p;
                    break;
                }
            }

            if (detailPart == null)
            {
                PartLoadStatus loadStatus;
                detailPart = theSession.Parts.OpenDisplay(partPath, out loadStatus);
                loadStatus.Dispose();
            }

            FileNew fileNew = theSession.Parts.FileNew();
            fileNew.TemplateFileName = "sts-A3.prt";
            fileNew.TemplatePresentationName = "STS - A3";
            fileNew.TemplateType = FileNewTemplateType.Item;
            fileNew.ApplicationName = "DrawingTemplate";
            fileNew.Units = Part.Units.Millimeters;
            fileNew.NewFileName = drawingPath;
            fileNew.MasterFileName = partPath;
            fileNew.UseBlankTemplate = false;
            fileNew.UsesMasterModel = "yes";
            fileNew.MakeDisplayedPart = true;
            fileNew.DisplayPartOption = DisplayPartOption.AllowAdditional;
            fileNew.Commit();
            fileNew.Destroy();

            Part drawingPart = theSession.Parts.Work;
            drawingPart.Drafting.EnterDraftingApplication();
            drawingPart.Drafting.SetTemplateInstantiationIsComplete(true);

            // === Skapa vyer via wizard ===
            var wizard = drawingPart.DraftingViews.CreateViewCreationWizardBuilder();
            wizard.Part = detailPart;
            wizard.BaseView = "FRONT";

            // ➕ Lägg till flera vyer
            wizard.RightView = true;
            wizard.TopView = true;
            wizard.IsometricView = true;

            // ➕ PMI och skalning
            wizard.InheritPMI = 3;
            wizard.InheritPmiOntoDrawing = true;
            wizard.PmiDimensionFromRevolved = true;
            wizard.ViewScale.Numerator = scale;

            // ➕ Automatisk grupplacering runt en mittpunkt
            wizard.PlacementOption = NXOpen.Drawings.ViewCreationWizardBuilder.Option.Manual;
            wizard.MultipleViewPlacement.OptionType = NXOpen.Drawings.MultipleViewPlacementBuilder.Option.Center;

            // Sätt placeringen mitt på ritningen (justera gärna!)
            Point3d centerPoint = new Point3d(200, 190, 0);
            wizard.MultipleViewPlacement.ViewPlacementCenter.Placement.SetValue(null, drawingPart.Views.WorkView, centerPoint);

            wizard.Commit();
            wizard.Destroy();

            // === Lägg till not om skala ===
            NXOpen.Annotations.DraftingNoteBuilder noteBuilder = drawingPart.Annotations.CreateDraftingNoteBuilder(null);
            noteBuilder.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;
            noteBuilder.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
            noteBuilder.Origin.Origin.SetValue(null, null, new Point3d(85, 90, 0));
            string[] noteText = new string[1];
            noteText[0] = $"SCALE 1:{scale}";
            noteBuilder.Text.TextBlock.SetText(noteText);
            noteBuilder.Commit();
            noteBuilder.Destroy();


        }





        // Anropa separat för varje vy
        //SetPMIForView(drawingPart, frontView);
        //   SetPMIForView(drawingPart, rightView);
        //   SetPMIForView(drawingPart, isoView);

        // === Måttsättning för Lower_brac_Picatinny_1 ===
        // Mått 1: Höjd (t.ex. 31.8 mm)
        //AddDimensionBetweenCurves(115, 85, drawingPart, 1, 2, frontView);

        //// Mått 2: Total höjd (t.ex. 37.2 mm)
        //AddDimensionBetweenCurves(115, 50, drawingPart, 2, 3, frontView);

        //// === Måttsättning för Upper_brac_Picatinny_1 ===
        //// Mått 1: Spårhöjd (t.ex. 0.51 mm)
        //AddDimensionBetweenCurves(115, 55, drawingPart, 1, 2, frontView);

        //// Mått 2: Totalhöjd (t.ex. 5.33 mm)
        //AddDimensionBetweenCurves(115, 90, drawingPart, 2, 3, frontView);


        private void AddDimensionBetweenCurves(
            double posX,
            double posY,
            Part part,
            int curveIndex1,
            int curveIndex2,
            BaseView view)
        {
            RapidDimensionBuilder dimBuilder = part.Dimensions.CreateRapidDimensionBuilder(null);
            dimBuilder.Driving.DrivingMethod = DrivingValueBuilder.DrivingValueMethod.Reference;

            // Hämta kurvorna i den valda vyn
            DraftingBody body = view.DraftingBodies.ToArray()[0];
            DraftingCurve curve1 = body.DraftingCurves.ToArray()[curveIndex1 - 1];
            DraftingCurve curve2 = body.DraftingCurves.ToArray()[curveIndex2 - 1];

            // Dummy positions (krävs men används inte eftersom vi ställer in riktig origin)ss
            Point3d dummy = new Point3d(0, 0, 0);

            // Plats där dimensionen ska hamna
            Point3d dimensionLocation = new Point3d(posX, posY, 0);

            // Sätt associeringar
            dimBuilder.FirstAssociativity.SetValue(InferSnapType.SnapType.End, curve1, view, dummy, null, null, dummy);
            dimBuilder.SecondAssociativity.SetValue(InferSnapType.SnapType.End, curve2, view, dummy, null, null, dummy);

            // 🔧 Manuell position
            dimBuilder.Origin.SetInferRelativeToGeometry(false);
            dimBuilder.Origin.Origin.SetValue(null, null, dimensionLocation);

            dimBuilder.Commit();
            dimBuilder.Destroy();
        }
        public void HideDatumsAndSketches1()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Edit->Show and Hide->Show and Hide...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            theSession.SetUndoMarkName(markId1, "Show and Hide Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Hide Sketches");

            int numberHidden1;
            numberHidden1 = theSession.DisplayManager.HideByType("SHOW_HIDE_TYPE_SKETCHES", NXOpen.DisplayManager.ShowHideScope.AnyInAssembly);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            workPart.Views.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Hide Datums");

            int numberHidden2;
            numberHidden2 = theSession.DisplayManager.HideByType("SHOW_HIDE_TYPE_DATUMS", NXOpen.DisplayManager.ShowHideScope.AnyInAssembly);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);

            workPart.Views.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Hide Points");

            int numberHidden3;
            numberHidden3 = theSession.DisplayManager.HideByType("SHOW_HIDE_TYPE_POINTS", NXOpen.DisplayManager.ShowHideScope.AnyInAssembly);

            int nErrs3;
            nErrs3 = theSession.UpdateManager.DoUpdate(markId4);

            workPart.Views.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);

            theSession.SetUndoMarkName(markId1, "Show and Hide");

            theSession.DeleteUndoMark(markId1, null);

            // ----------------------------------------------
            //   Menu: Edit->View->Update...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Drawings.UpdateViewsBuilder updateViewsBuilder1;
            updateViewsBuilder1 = workPart.DraftingViews.CreateUpdateViewsBuilder();

            theSession.SetUndoMarkName(markId5, "Update Views Dialog");

            NXOpen.Drawings.BaseView baseView1 = ((NXOpen.Drawings.BaseView)workPart.DraftingViews.FindObject("Front@1"));
            bool added1;
            added1 = updateViewsBuilder1.Views.Add(baseView1);

            NXOpen.Drawings.BaseView baseView2 = ((NXOpen.Drawings.BaseView)workPart.DraftingViews.FindObject("Right@2"));
            bool added2;
            added2 = updateViewsBuilder1.Views.Add(baseView2);

            NXOpen.Drawings.BaseView baseView3 = ((NXOpen.Drawings.BaseView)workPart.DraftingViews.FindObject("Isometric@3"));
            bool added3;
            added3 = updateViewsBuilder1.Views.Add(baseView3);

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Views");

            theSession.DeleteUndoMark(markId6, null);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Views");

            NXOpen.NXObject nXObject1;
            nXObject1 = updateViewsBuilder1.Commit();

            theSession.DeleteUndoMark(markId7, null);

            theSession.SetUndoMarkName(markId5, "Update Views");

            updateViewsBuilder1.Destroy();
        }
    }
}
