using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace GiaoHang.Table_HangHoa
{
    [ToolboxItemAttribute(false)]
    public class Table_HangHoa : WebPart
    {
        GiaoHangEntities data = new GiaoHangEntities();
        
        Label Lb_Find = new Label();
        Label Lb_Name = new Label();
        Label Lb_KG = new Label();
        Label Lb_Img = new Label();
        Label Lb_SL = new Label();
        Label Lb_Loai = new Label();
        Label Val = new Label();
        TextBox Find = new TextBox();
        Table table = new Table();
        protected override void CreateChildControls()
        {
            TableRow tr;
            tr = new TableRow();
            Lb_Find.Text = "Tim Kiem:";
            Lb_Find.Attributes["style"] = " width: 20%;margin-right:10px;";
            Find.Attributes["style"] = " width:40% ;height: 20px;margin-top:10px;margin-bottom: 10px;";
            Button Look = new Button();
            Look.Text = "Tim";
            Look.Click += Look_Click;
            Button ALL = new Button();
            ALL.Text = "All";
            ALL.Click += ALL_Click;
            this.Controls.Add(Lb_Find);
            this.Controls.Add(Find);
            this.Controls.Add(Look);
            this.Controls.Add(ALL);
            addStyle();
            AddTable(null);
        }

        private void ALL_Click(object sender, EventArgs e)
        {
            AddTable(null);
        }

        private void Look_Click(object sender, EventArgs e)
        {
            AddTable(Find.Text);
        }

        private void AddTable(string find)
        {
            table.Controls.Clear();
            table.Attributes["style"] = "border-collapse: collapse;width: 90%;";
            TableRow tr;
            tr = new TableRow();
            tr.Attributes["style"] = "width:80% ;";
            addColumn(tr, Lb_Name);
            addColumn(tr, Lb_KG);
            addColumn(tr, Lb_SL);
            addColumn(tr, Lb_Img);
            addColumn(tr, Lb_Loai);
            table.Controls.Add(tr);
            var lst = data.getNameHangHoa(find).ToList();
            foreach (var item in lst)
            {
                tr = new TableRow();
                tr.Attributes["style"] = "width:80% ;";
                addVal(tr, item.TenHH);
                addVal(tr, item.KG.ToString());
                addVal(tr, item.SL.ToString());
                if (item.Img != null)
                {
                    MemoryStream ms = new MemoryStream(item.Img);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    Image anh = new Image();
                    anh.Attributes["style"] = "width:100px;";
                    string base64ImageString = Convert.ToBase64String(ImageToByteArray(img));
                    anh.ImageUrl = string.Format("data:image/jpg;base64,{0}", base64ImageString);
                    TableCell tc;
                    tc = new TableCell();
                    tc.Attributes["style"] = "width:20% ;border: 1px solid black;text-align: center; ";
                    tc.Controls.Add(anh);
                    tr.Controls.Add(tc);
                }
                else
                    addVal(tr, "test");
                LoaiHH temp = data.LoaiHHs.FirstOrDefault(n => n.MaLHH == item.MaLHH);
                addVal(tr, temp.TenLHH);
                table.Controls.Add(tr);
            }
            this.Controls.Add(table);
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private void addVal(TableRow tr,string text)
        {
            Label val = new Label();
            TableCell tc;
            tc = new TableCell();
            val.Text = text;
            tc.Attributes["style"] = "width:20% ;border: 1px solid black;text-align: center; ";
            tc.Controls.Add(val);
            tr.Controls.Add(tc);
        }
        private void addColumn(TableRow tr , Control val)
        {
            TableCell tc;
            tc = new TableCell();
            tc.Attributes["style"] = "width:20% ;border: 1px solid black;text-align: center;font-weight: bolder;";
            tc.Controls.Add(val);
            tr.Controls.Add(tc);
        }
        public void addStyle() // them css cho cac Controls
        {
            Lb_Name.Text = "Tên HH";
            Lb_KG.Text = "KG";
            Lb_Img.Text = "Img";
            Lb_SL.Text = "Số lượng";
            Lb_Loai.Text = "Loại";
        }
    }
}
