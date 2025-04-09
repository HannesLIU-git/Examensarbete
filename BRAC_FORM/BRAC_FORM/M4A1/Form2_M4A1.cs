using NXOpen;
using NXOpen.Annotations;
using NXOpen.Drawings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BRAC_FORM
{
    public partial class Form2_M4A1: Form
    {
        string originalTextbox1 = "";
        string originalTextbox2 = "";
        string originalTextbox3 = "";
        string originalTextbox4 = "";
        public Form2_M4A1()
        {
            InitializeComponent();
            button4.Enabled = false;
            button6.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e) ////////////////// ADD BRACKET
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.bracketCounter++;

            string D_width = GlobalVariables.PipeDiameter;
            GlobalVariables.Width = textBox1.Text;
            string Width = GlobalVariables.Width;
            string XPos = "10";
            string YPos = "10";

            int IntPoint = (int)GlobalVariables.barrelEnd;

            GlobalVariables.BracketPos = textBox2.Text;
            int bracketPos = int.Parse(GlobalVariables.BracketPos);

            double Pos = IntPoint + bracketPos;

            int Forkint = (int)GlobalVariables.forkdistance + 1;
            //string Gaffel_W = Forkint.ToString();

            int bracketWidth = int.Parse(Width);

            int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
            int FrontDif = Math.Abs(IntPoint - Frontint);
            int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


           
            string Gaffel_L = textBox3.Text;
            string Gaffel_W = textBox4.Text;


            //GlobalVariables.FilePath = GlobalVariables.FilePath + "\\Nya_CAD";
            Point3d position = new Point3d(0.0, Pos, 0.0);
            string partsFolderPath = GlobalVariables.FilePath;
            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Locking_brack.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Locking_Pin.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Lower_brac_new_m16.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("M6_35_NEW_BRAC.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("RPD_PIN.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("SAT_FUNK.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_NEW_clamp.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);


            addItem.updateAll();
            addItem.HideDatumsAndSketches();

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket added at {position}.");

            //GlobalVariables.FilePath = GlobalVariables.FilePath.Replace("\\Nya_CAD", "");

            originalTextbox1 = textBox1.Text;
            originalTextbox2 = textBox2.Text;

            button4.Enabled = true;
            button3.Enabled = false;
            button6.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e) ///////////////////////// DELETE
        {
            Class_Add_item addItem = new Class_Add_item();
            
            //GlobalVariables.FilePath = GlobalVariables.FilePath + "\\Nya_CAD";

            //UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, $"{GlobalVariables.FilePath}");

            addItem.DeleteBracket("Locking_brack", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Locking_Pin", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Lower_brac_new_m16", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("M6_35_NEW_BRAC", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("RPD_PIN", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("SAT_FUNK", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_NEW_clamp", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");
           
            //GlobalVariables.FilePath = GlobalVariables.FilePath.Replace("\\Nya_CAD", "");

            button4.Enabled = false;
            button3.Enabled = true;
            button6.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) ///////////////////// PREVIOUS
        {
            Form1_CLAMP form1_M4A1 = new Form1_CLAMP(); // Create an instance of Form1
            form1_M4A1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button2_Click(object sender, EventArgs e) ////////////////////// NEXT
        {

        }

        private void button5_Click(object sender, EventArgs e)
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

            string filename = GlobalVariables.FilePath + "\\assembly1_dwg1.prt";
            fileNew1.NewFileName =filename;

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
            CreateSeparateDetailDrawing("M6_35_NEW_BRAC_1");
            CreateSeparateDetailDrawing("Upper_NEW_clamp_1");
            CreateSeparateDetailDrawing("Lower_brac_new_m16_1");
            CreateSeparateDetailDrawing("Locking_brack_1");
            CreateSeparateDetailDrawing("RPD_PIN_1");

            MessageBox.Show("Alla ritningar skapade!");

            //theSession.ApplicationSwitchImmediate("UG_APP_MODELING"); Denna ska fixa saker men fixar inget


        }

        private void CreateSeparateDetailDrawing(string partName)
        {
            Session theSession = Session.GetSession();
            string baseDir = @"C:\Users\u107284\Desktop\REEPOE\BRAC_FORM\CAD\";
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
            fileNew.MakeDisplayedPart = true;
            fileNew.DisplayPartOption = DisplayPartOption.AllowAdditional;
            fileNew.Commit();
            fileNew.Destroy();

            Part drawingPart = theSession.Parts.Work;
            drawingPart.Drafting.EnterDraftingApplication();
            drawingPart.Drafting.SetTemplateInstantiationIsComplete(true);

            ModelingView front = drawingPart.ModelingViews.FindObject("Front") as ModelingView;
            ModelingView right = drawingPart.ModelingViews.FindObject("Right") as ModelingView;
            ModelingView iso = drawingPart.ModelingViews.FindObject("Isometric") as ModelingView;

            BaseViewBuilder frontBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            frontBuilder.SelectModelView.SelectedView = front;
            frontBuilder.Placement.Placement.SetValue(null, drawingPart.Views.WorkView, new Point3d(100, 100, 0));
            BaseView frontView = (BaseView)frontBuilder.Commit();
            frontBuilder.Destroy();

            BaseViewBuilder rightBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            rightBuilder.SelectModelView.SelectedView = right;
            rightBuilder.Placement.Placement.SetValue(frontView, drawingPart.Views.WorkView, new Point3d(220, 100, 0));
            BaseView rightView = (BaseView)rightBuilder.Commit();
            rightBuilder.Destroy();

            BaseViewBuilder isoBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            isoBuilder.SelectModelView.SelectedView = iso;
            isoBuilder.Placement.Placement.SetValue(null, drawingPart.Views.WorkView, new Point3d(250, 210, 0));
            BaseView isoView = (BaseView)isoBuilder.Commit();
            isoBuilder.Destroy();

            // === Måttsättning för Lower_brac_Picatinny_1 ===
            // Mått 1: Höjd (t.ex. 31.8 mm)
            AddDimensionBetweenCurves(115, 85, drawingPart, 1, 2, frontView);

            // Mått 2: Total höjd (t.ex. 37.2 mm)
            AddDimensionBetweenCurves(115, 50, drawingPart, 2, 3, frontView);

            // === Måttsättning för Upper_brac_Picatinny_1 ===
            // Mått 1: Spårhöjd (t.ex. 0.51 mm)
            AddDimensionBetweenCurves(115, 55, drawingPart, 1, 2, frontView);

            // Mått 2: Totalhöjd (t.ex. 5.33 mm)
            AddDimensionBetweenCurves(115, 90, drawingPart, 2, 3, frontView);

        }
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

        private void button6_Click(object sender, EventArgs e) ////////////////////////// UPDATE
        {
            if (textBox1.Text != originalTextbox1 || textBox2.Text != originalTextbox2 || textBox3.Text != originalTextbox3 || textBox3.Text != originalTextbox3)
            {
                Class_Add_item addItem = new Class_Add_item();
                addItem.DeleteBracket("Locking_brack", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Locking_Pin", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Lower_brac_new_m16", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("M6_35_NEW_BRAC", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("RPD_PIN", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("SAT_FUNK", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Upper_NEW_clamp", GlobalVariables.bracketCounter);

                Session theSession = Session.GetSession();
                Part assemblyPart = (Part)theSession.Parts.Work;

                GlobalVariables.bracketCounter++;

                string D_width = GlobalVariables.PipeDiameter;
                GlobalVariables.Width = textBox1.Text;
                string Width = GlobalVariables.Width;

                string XPos = "10";
                string YPos = "10";

                int IntPoint = (int)GlobalVariables.barrelEnd;

                GlobalVariables.BracketPos = textBox2.Text;
                int bracketPos = int.Parse(GlobalVariables.BracketPos);

                double Pos = IntPoint + bracketPos;

                int Forkint = (int)GlobalVariables.forkdistance + 1;
                //string Gaffel_W = Forkint.ToString();

                int bracketWidth = int.Parse(Width);

                int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
                int FrontDif = Math.Abs(IntPoint - Frontint);
                int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


                //string Gaffel_L = FrontPOS.ToString();
                string Gaffel_L = textBox3.Text;
                string Gaffel_W = textBox4.Text;


                //GlobalVariables.FilePath = GlobalVariables.FilePath + "\\Nya_CAD";
                Point3d position = new Point3d(0.0, Pos, 0.0);
                string partsFolderPath = GlobalVariables.FilePath;

                addItem.AddPartToAssembly("Locking_brack.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Locking_Pin.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Lower_brac_new_m16.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("M6_35_NEW_BRAC.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("RPD_PIN.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("SAT_FUNK.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Upper_NEW_clamp.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);


                addItem.updateAll();
                addItem.HideDatumsAndSketches();

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket updated");
                
                button6.Enabled = true;
            }
            else 
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Information, $"Please change any parameter.");
            }
        }
    }
}
