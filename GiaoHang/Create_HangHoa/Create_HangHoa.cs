using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace GiaoHang.Create_HangHoa
{
    [ToolboxItemAttribute(false)]
    public class Create_HangHoa : WebPart
    {
        
        Label Lb_Name = new Label();
        Label Lb_KG = new Label();
        Label Lb_Img = new Label();
        Label Lb_SL = new Label();
        Label Lb_Loai = new Label();
        TextBox Name = new TextBox();
        TextBox KG = new TextBox();
        TextBox SL = new TextBox();
        DropDownList Loai = new DropDownList();
        FileUpload IMG = new FileUpload();
        Table table = new Table();

        GiaoHangEntities data = new GiaoHangEntities();
        protected override void CreateChildControls()
        {
            Controls.Clear();
            Add_Column();
            Add_Table();
        }
        public void addStyle() // them css cho cac Controls
        {
            Lb_Name.Text = "Tên HH";
            Lb_KG.Text = "KG";
            Lb_Img.Text = "Img";
            Lb_SL.Text = "Số lượng";
            Lb_Loai.Text = "Loại";
            Lb_Name.Attributes["style"] = "float:left; width: 15%; margin - top: 5px;";
            Lb_KG.Attributes["style"] = "float:left; width: 15%; margin - top: 5px;";
            Lb_Img.Attributes["style"] = "float:left; width: 15%; margin - top: 5px;";
            Lb_SL.Attributes["style"] = "float:left; width: 15%; margin - top: 5px;";
            Lb_Loai.Attributes["style"] = "float:left; width: 15%; margin - top: 5px;";
            Name.Attributes["style"] = "width:80% ;height: 20px;margin-top:10px;";
            Name.ID = "TenHH";
            KG.Attributes["style"] = "width:80% ;height: 20px;margin-top:10px;";
            SL.Attributes["style"] = "width:80% ;height: 20px;margin-top:10px;";
            Loai.Attributes["style"] = "width:80% ;height: 20px;margin-top:10px;";
        }
        protected void Add_Table()
        {
            addStyle();
            AddControl(Lb_Name, table, Name,"Khong duoc de trong ten hang hoa!");
            KG.TextMode = TextBoxMode.Number;
            AddControl(Lb_KG, table, KG,null);
            SL.TextMode = TextBoxMode.Number;
            AddControl(Lb_SL, table, SL,null);
            IMG.Attributes["accept"] = "image/jpeg";
            AddAttachments(Lb_Img, table, IMG);
            AddControl(Lb_Loai, table, Loai,null);
            this.Controls.Add(table);
            Button save = new Button();
            save.Text = "Save";
            save.Click += Save_Click;
            this.Controls.Add(save);
            ChildControlsCreated = true;
        }
        protected void Add_Column()
        {
            using (SPSite oSPStie = new SPSite("http://localhost:212/"))
            {
                using (SPWeb oSPWeb = oSPStie.OpenWeb())
                {
                    oSPWeb.AllowUnsafeUpdates = true;
                    SPList list = oSPWeb.Lists["HangHoa"];
                    CreateText(list, "TenHH");
                    CreateNumber(list, "KG");
                    CreateNumber(list, "SL");
                    CreateChoice(list, "LoaiHH");
                    SPFieldChoice choice = (SPFieldChoice)list.Fields.GetFieldByInternalName("LoaiHH");
                    foreach (var item in choice.Choices)
                    {
                        Loai.Items.Add(item);
                    }
                }
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            Save_Item();
        }
        private void Save_Item()
        {
            using (SPSite oSPStie = new SPSite("http://localhost:212/"))
            {
                using (SPWeb oSPWeb = oSPStie.OpenWeb())
                {
                    oSPWeb.AllowUnsafeUpdates = true;
                    SPList list = oSPWeb.Lists["HangHoa"];
                    SPListItem itemToAdd = list.Items.Add();
                    itemToAdd["TenHH"] = Convert.ToString(Name.Text);
                    itemToAdd["KG"] = Convert.ToString(KG.Text);
                    itemToAdd["SL"] = Convert.ToString(SL.Text);
                    itemToAdd["LoaiHH"] = Convert.ToString(Loai.SelectedValue.ToString());
                    itemToAdd.Update();
                    itemToAdd["Title"] = itemToAdd.ID;
                    SaveFile(oSPWeb,itemToAdd);
                    itemToAdd.Update();
                    HangHoa emp = new HangHoa();
                    emp.TenHH = Name.Text;
                    emp.KG = int.Parse(KG.Text);
                    emp.SL = int.Parse(SL.Text);
                    System.Drawing.Image imag = System.Drawing.Image.FromStream(IMG.PostedFile.InputStream);
                    //Stream fStream = IMG.PostedFile.InputStream;
                    //byte[] contents = new byte[fStream.Length];
                    //fStream.Read(contents, 0, (int)fStream.Length);
                    emp.Img = ConvertImageToByteArray(imag, System.Drawing.Imaging.ImageFormat.Jpeg);
                    LoaiHH temp = data.LoaiHHs.FirstOrDefault(n => n.TenLHH.Equals(Loai.SelectedValue.ToString()));
                    emp.MaLHH =temp.MaLHH;
                    data.HangHoas.Add(emp);
                    data.SaveChanges();
                    oSPWeb.AllowUnsafeUpdates = false;
                    oSPWeb.Dispose();
                }
            }
        }
        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,
                                       System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }
        public void AddControl(Label lable, Table model, Control textbox,string validate)
        {
            TableRow tr;
            TableCell tc;
            tr = new TableRow();
            tc = new TableCell();
            tr.Attributes["style"] = "width:80% ;";
            tc.Attributes["style"] = "width:15% ;";
            tc.Controls.Add(lable);
            tr.Controls.Add(tc);
            tc = new TableCell();
            tc.Attributes["style"] = "width:80% ;";
            tc.Controls.Add(textbox);
            tr.Controls.Add(tc);
            if(validate != null)
            {
                addValidate(textbox, tr, validate);
            }
            model.Controls.Add(tr);
        }
        private void addValidate(Control textbox, TableRow tr,string sms)
        {
            TableCell tc;
            tc = new TableCell();
            RequiredFieldValidator validate = new RequiredFieldValidator();
            validate.ControlToValidate = textbox.ID;
            validate.ErrorMessage = sms;
            validate.ForeColor = Color.Red;
            tc.Controls.Add(validate);
            tr.Controls.Add(tc);
        }
        //Create Text
        private void CreateText(SPList list, string Displayname)
        {
            if (!list.Fields.ContainsField(Displayname))
            {
                list.Fields.Add(Displayname, SPFieldType.Text, true);
                list.Update();
                SPFieldText nb = (SPFieldText)list.Fields.GetFieldByInternalName(Displayname);
                nb.Title = Displayname;
                nb.Update();
                SPView view = list.DefaultView;
                view.ViewFields.Add(Displayname);
                view.Update();
            }
        }
        //Create Number
        private void CreateNumber(SPList list, string Displayname)
        {
            if (!list.Fields.ContainsField(Displayname))
            {
                list.Fields.Add(Displayname, SPFieldType.Number, true);
                list.Update();
                SPFieldNumber nb = (SPFieldNumber)list.Fields.GetFieldByInternalName(Displayname);
                nb.Title = Displayname;
                nb.Update();
                SPView view = list.DefaultView;
                view.ViewFields.Add(Displayname);
                view.Update();
            }
        }
        //Create Choice
        private void CreateChoice(SPList list, string Displayname)
        {
            if (!list.Fields.ContainsField(Displayname))
            {
                list.Fields.Add(Displayname, SPFieldType.Choice, true);
                list.Update();
                SPFieldChoice nb = (SPFieldChoice)list.Fields.GetFieldByInternalName(Displayname);
                nb.Title = Displayname;
                var lstLoai = data.LoaiHHs.ToList();
                foreach(var item in lstLoai)
                {
                    nb.AddChoice(item.TenLHH);
                }
                nb.Update();
                SPView view = list.DefaultView;
                view.ViewFields.Add(Displayname);
                view.Update();
            }
        }
        public void AddAttachments(Label lable, Table model, Control textbox)
        {
            TableRow tr;
            TableCell tc;
            tr = new TableRow();
            tc = new TableCell();
            tr.Attributes["style"] = "width:80% ;";
            tc.Attributes["style"] = "width:15% ;";
            tc.Controls.Add(lable);
            tr.Controls.Add(tc);
            tc = new TableCell();
            //Load_lst_file(tr);
            tc.Attributes["style"] = "width:80% ;";
            tc.Controls.Add(textbox);
            tr.Controls.Add(tc);
            model.Controls.Add(tr);
        }
        private void SaveFile(SPWeb web, SPListItem newItem)
        {
            Stream fStream = IMG.PostedFile.InputStream;
            byte[] contents = new byte[fStream.Length];
            fStream.Read(contents, 0, (int)fStream.Length);
            SPSecurity.CatchAccessDeniedException = false;
            SPList DocLib = web.Lists["Documents"];
            if (!web.GetFolder(DocLib.RootFolder.Url + $"/{newItem.ID}").Exists)
            {
                SPListItem folderColl = DocLib.Items.Add(DocLib.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, $"{newItem.ID}");
                folderColl["Title"] = $"{newItem.ID}";
                folderColl.Update();
            }

            SPFolder SubFolder = DocLib.RootFolder.SubFolders[$"{newItem.ID}"];
            //Add the file to the sub-folder
            SPFile file = SubFolder.Files.Add(SubFolder.Url + $"/{IMG.PostedFile.FileName}", contents, true);
            SubFolder.Update();
            newItem["IMG"] = new SPFieldLookupValue(SubFolder.Item.ID, SubFolder.Name);
            newItem.Update();
        }
        private void Load_lst_file(TableRow tr)
        {
            TableCell tc = new TableCell();
            string url = SPContext.Current.ListItem.Attachments.UrlPrefix;
            foreach (String item in SPContext.Current.ListItem.Attachments)
            {
                HyperLink HpL_File = new HyperLink();
                HpL_File.Text = item;
                HpL_File.NavigateUrl = url + item;
                tc.Controls.Add(HpL_File);
                Button delete = new Button();
                delete.Text = "Delete";
                delete.Click += (s, e) =>
                {
                    SPContext.Current.ListItem.Attachments.Delete(item);
                };
                tc.Controls.Add(delete);
                tr.Controls.Add(tc);
            }
        }
    }
}
