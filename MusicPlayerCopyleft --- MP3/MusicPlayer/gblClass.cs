using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.OleDb;

namespace MusicPlayerCopyleft
{

    class gblClass
    {

      
       public string MainMessage = "You are not authrosied user";
       



       public void fnFillComboBox(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet(mlsSql);
              // ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   //dr = ds.Tables[0].NewRow();
                   //dr[ValMember] = 0;
                   //dr[DispMember] = displayTextAtZeroIndex;
                  // ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   //Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
              // MessageBox.Show(ex.Message);
           }
       }
       public void fnFillAdvtComboBox(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet(mlsSql);
                ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = displayTextAtZeroIndex;
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
               // MessageBox.Show(ex.Message);
           }
       }

       public DataTable fnFillDataTable(string sSql)
       {
           SqlDataAdapter Adp = new SqlDataAdapter();
           DataTable mldData;
           try
           {
               Adp = new SqlDataAdapter(sSql, StaticClass.constr );
               mldData = new DataTable();
               Adp.Fill(mldData);
           }
           catch (Exception ex)
           {
                mldData = new DataTable();
              // MessageBox.Show(ex.Message);
           }
           return mldData;
       }
        public DataTable fnFillDataTable_OldDB(string sSql)
        {
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable mldData;
            try
            {
                Adp = new SqlDataAdapter(sSql, StaticClass.constrOldDB);
                mldData = new DataTable();
                Adp.Fill(mldData);
            }
            catch (Exception ex)
            {
                mldData = new DataTable();
                // MessageBox.Show(ex.Message);
            }
            return mldData;
        }
        public DataTable fnFillDataTable_Local(string sSql)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataTable mldData;
           try
           {
               Adp = new OleDbDataAdapter(sSql, StaticClass.LocalCon);
               mldData = new DataTable();
               Adp.Fill(mldData);
           }
           catch (Exception ex)
           {
               mldData = new DataTable();
              // MessageBox.Show(ex.Message);
           }
           return mldData;
       }

       public DataSet fnFillDataSet(string sQuery)
       {
           SqlDataAdapter Adp = new SqlDataAdapter();
           DataSet mlds;
           try
           {
               Adp = new SqlDataAdapter(sQuery, StaticClass.constr);
               mlds = new DataSet();
               Adp.Fill(mlds);
           }
           catch  (Exception ex)
           {
               mlds = new DataSet();
              // MessageBox.Show(ex.Message);
                
           }
           return mlds;
       }
       public DataSet fnFillDataSet_Local(string sQuery)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataSet mlds;
           try
           {
               Adp = new OleDbDataAdapter(sQuery, StaticClass.LocalCon);
               mlds = new DataSet();
               Adp.Fill(mlds);
           }
           catch(Exception ex)
           {
               mlds = new DataSet();
              // MessageBox.Show(ex.Message);

           }
           return mlds;
       }
        public DataSet fnFillDataSet_OldDB(string sQuery)
        {
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataSet mlds;
            try
            {
                Adp = new SqlDataAdapter(sQuery, StaticClass.constrOldDB);
                mlds = new DataSet();
                Adp.Fill(mlds);
            }
            catch (Exception ex)
            {
                mlds = new DataSet();
                // MessageBox.Show(ex.Message);

            }
            return mlds;
        }
        public void fnFillComboBox_OldDB(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
        {
            try
            {
                DataSet ds = new DataSet();
                // Warning!!! Optional parameters not supported
                DataRow dr;
                ds = fnFillDataSet_OldDB(mlsSql);
                // ds.Tables[0].DefaultView.Sort = DispMember;
                Combo.DataSource = null;
                if ((ds.Tables[0].Rows.Count > 0))
                {
                    //dr = ds.Tables[0].NewRow();
                    //dr[ValMember] = 0;
                    //dr[DispMember] = displayTextAtZeroIndex;
                    // ds.Tables[0].Rows.InsertAt(dr, 0);
                    Combo.ValueMember = ValMember;
                    Combo.DisplayMember = DispMember;
                    Combo.DataSource = ds.Tables[0];
                    Combo.Refresh();
                    //Combo.SelectedValue = 0;
                }
                else
                {
                    dr = ds.Tables[0].NewRow();
                    dr[ValMember] = 0;
                    dr[DispMember] = "";
                    ds.Tables[0].Rows.InsertAt(dr, 0);
                    Combo.ValueMember = ValMember;
                    Combo.DisplayMember = DispMember;
                    Combo.DataSource = ds.Tables[0];
                    Combo.Refresh();
                    Combo.SelectedValue = 0;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        public bool CheckForInternetConnection()
       {
           try
           {
               using (var client = new WebClient())
               using (var stream = client.OpenRead("http://www.google.com"))
               {
                   return true;
               }
           }
           catch
           {
               return false;
           }
       }

       public void DeleteAllOgg(string CurrentTitleId)
       {
           try
           {
               string d = Application.StartupPath;
               foreach (string f in Directory.GetFiles(d, "*.mp3"))
               {
                   FileInfo objFile = new FileInfo(f);
                   string FileName = objFile.Name;
                   if (CurrentTitleId != FileName)
                   {
                       File.Delete(f);
                   }
               }
           }
           catch (Exception ex) { }
       }

       public void fnFillComboBox_Local(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet_Local(mlsSql);
                ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = displayTextAtZeroIndex;
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   //Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
               // MessageBox.Show(ex.Message);
           }
       }
    }
}
