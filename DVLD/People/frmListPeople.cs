using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.ComponentModel; // استيراد مكتبة ComponentModel للخصائص والسمات
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Drawing; // استيراد مكتبة Drawing للتعامل مع الرسومات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Security.Policy; // استيراد مكتبة Security.Policy للسياسات الأمنية
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية
using System.Xml.Linq; // استيراد مكتبة Xml.Linq للتعامل مع XML
using DVLD.Classes; // استيراد مساحة الاسم الخاصة بالكلاسات العامة
using DVLD_Buisness; // استيراد طبقة الأعمال (Business Layer)
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView; // استيراد static من ListView

namespace DVLD.People // مساحة الاسم الخاصة بنماذج الأشخاص
{
    public partial class frmListPeople : Form // نموذج قائمة الأشخاص (partial class لأن Designer.cs يحتوي على باقي الكود)
    {

      // جدول بيانات ثابت يحتوي على جميع الأشخاص (يتم تحميله مرة واحدة)
      private static DataTable _dtAllPeople = clsPerson.GetAllPeople();
        
        // فقط اختر الأعمدة التي تريد عرضها في الشبكة
        // إنشاء جدول بيانات يحتوي على الأعمدة المحددة فقط
      private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

        // دالة خاصة لتحديث قائمة الأشخاص
        private void _RefreshPeoplList()
        {
            _dtAllPeople = clsPerson.GetAllPeople(); // إعادة تحميل جميع الأشخاص من قاعدة البيانات
            // إعادة إنشاء جدول البيانات بالأعمدة المحددة فقط
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople; // ربط جدول البيانات بالشبكة (DataGridView)
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString(); // عرض عدد السجلات
        }

        // Constructor للنموذج
        public frmListPeople()
        {
            InitializeComponent(); // تهيئة مكونات النموذج
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmListPeople_Load(object sender, EventArgs e)
        {
                 
            dgvPeople.DataSource = _dtPeople; // ربط جدول البيانات بالشبكة
            cbFilterBy.SelectedIndex = 0; // تعيين الفلتر الافتراضي (الخيار الأول)
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString(); // عرض عدد السجلات
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }

        }
    
        // حدث تغيير نص حقل البحث (Filter)
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
           
            string FilterColumn = ""; // متغير لتخزين اسم العمود المراد الفلترة عليه
            // ربط الفلتر المحدد باسم العمود الحقيقي في قاعدة البيانات
            switch (cbFilterBy.Text)
            {
                case "Person ID": // إذا كان الفلتر "Person ID"
                    FilterColumn = "PersonID"; // اسم العمود الحقيقي
                    break;

                case "National No.": // إذا كان الفلتر "National No."
                    FilterColumn = "NationalNo"; // اسم العمود الحقيقي
                    break;

                case "First Name": // إذا كان الفلتر "First Name"
                    FilterColumn = "FirstName"; // اسم العمود الحقيقي
                    break;

                case "Second Name": // إذا كان الفلتر "Second Name"
                    FilterColumn = "SecondName"; // اسم العمود الحقيقي
                    break;

                case "Third Name": // إذا كان الفلتر "Third Name"
                    FilterColumn = "ThirdName"; // اسم العمود الحقيقي
                    break;

                case "Last Name": // إذا كان الفلتر "Last Name"
                    FilterColumn = "LastName"; // اسم العمود الحقيقي
                    break;

                case "Nationality": // إذا كان الفلتر "Nationality"
                    FilterColumn = "CountryName"; // اسم العمود الحقيقي
                    break;

                case "Gendor": // إذا كان الفلتر "Gendor"
                    FilterColumn = "GendorCaption"; // اسم العمود الحقيقي
                    break;

                case "Phone": // إذا كان الفلتر "Phone"
                    FilterColumn = "Phone"; // اسم العمود الحقيقي
                    break;

                case "Email": // إذا كان الفلتر "Email"
                    FilterColumn = "Email"; // اسم العمود الحقيقي
                    break;

                default: // في حالة أخرى
                    FilterColumn = "None"; // لا يوجد فلتر
                    break;

            }

            // إعادة تعيين الفلاتر في حالة عدم اختيار شيء أو أن قيمة الفلتر فارغة
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = ""; // إزالة الفلتر (عرض جميع السجلات)
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString(); // تحديث عدد السجلات
                return; // إنهاء الدالة
            }


            if (FilterColumn == "PersonID") // إذا كان العمود المراد الفلترة عليه هو PersonID
                // في هذه الحالة نتعامل مع رقم صحيح وليس نص
                
              _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام = (مطابقة تامة)
            else // إذا كان العمود نص
             _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام LIKE (يبدأ بـ)
       
         lblRecordsCount.Text= dgvPeople.Rows.Count.ToString(); // تحديث عدد السجلات بعد الفلترة

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

           txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Form frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeoplList();

        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeoplList();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm1 = new frmAddUpdatePerson();
            frm1.ShowDialog();
            _RefreshPeoplList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cbFilterBy.Text=="Person ID")
              e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
