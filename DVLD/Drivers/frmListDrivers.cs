using DVLD.Licenses.International_License;
using DVLD.People;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtAllDrivers;

        public frmListDrivers()
        {
            InitializeComponent();
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج

        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0; // تعيين الفلتر الافتراضي (الخيار الأول)
            // الحصول على جميع السائقين من قاعدة البيانات
            _dtAllDrivers = clsDriver.GetAllDrivers();
            // ربط جدول البيانات بالشبكة (DataGridView)
            dgvDrivers.DataSource = _dtAllDrivers;
            // عرض عدد السجلات
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
            // إذا كانت هناك سجلات في الشبكة
            if (dgvDrivers.Rows.Count>0)
            {
                // تخصيص أعمدة الشبكة (العناوين والعرض)
                dgvDrivers.Columns[0].HeaderText = "Driver ID"; // عنوان العمود الأول
                dgvDrivers.Columns[0].Width = 120; // عرض العمود الأول

                dgvDrivers.Columns[1].HeaderText = "Person ID"; // عنوان العمود الثاني
                dgvDrivers.Columns[1].Width = 120; // عرض العمود الثاني

                dgvDrivers.Columns[2].HeaderText = "National No."; // عنوان العمود الثالث
                dgvDrivers.Columns[2].Width = 140; // عرض العمود الثالث

                dgvDrivers.Columns[3].HeaderText = "Full Name"; // عنوان العمود الرابع
                dgvDrivers.Columns[3].Width = 320; // عرض العمود الرابع

                dgvDrivers.Columns[4].HeaderText = "Date"; // عنوان العمود الخامس
                dgvDrivers.Columns[4].Width = 170; // عرض العمود الخامس

                dgvDrivers.Columns[5].HeaderText = "Active Licenses"; // عنوان العمود السادس
                dgvDrivers.Columns[5].Width = 150; // عرض العمود السادس
            }
          


        }

        // حدث تغيير اختيار نوع الفلتر
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // إظهار حقل البحث النصي إذا كان الفلتر ليس "None"
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
         

            if (cbFilterBy.Text == "None") // إذا كان الفلتر "None"
            {
                txtFilterValue.Enabled = false; // تعطيل حقل البحث
            }
            else // إذا كان الفلتر نوع آخر
                txtFilterValue.Enabled = true; // تفعيل حقل البحث

            txtFilterValue.Text = ""; // مسح نص البحث
            txtFilterValue.Focus(); // نقل التركيز إلى حقل البحث
        }

        // حدث تغيير نص حقل البحث (Filter)
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = ""; // متغير لتخزين اسم العمود المراد الفلترة عليه
            // ربط الفلتر المحدد باسم العمود الحقيقي في قاعدة البيانات
            switch (cbFilterBy.Text)
            {
                case "Driver ID": // إذا كان الفلتر "Driver ID"
                    FilterColumn = "DriverID"; // اسم العمود الحقيقي
                    break;

                case "Person ID": // إذا كان الفلتر "Person ID"
                    FilterColumn = "PersonID"; // اسم العمود الحقيقي
                    break;

                case "National No.": // إذا كان الفلتر "National No."
                    FilterColumn = "NationalNo"; // اسم العمود الحقيقي
                    break;


                case "Full Name": // إذا كان الفلتر "Full Name"
                    FilterColumn = "FullName"; // اسم العمود الحقيقي
                    break;

                default: // في حالة أخرى
                    FilterColumn = "None"; // لا يوجد فلتر
                    break;

            }

            // إعادة تعيين الفلاتر في حالة عدم اختيار شيء أو أن قيمة الفلتر فارغة
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDrivers.DefaultView.RowFilter = ""; // إزالة الفلتر (عرض جميع السجلات)
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString(); // تحديث عدد السجلات
                return; // إنهاء الدالة
            }


            // إذا كان العمود رقم (ليس نص)
            if (FilterColumn != "FullName" && FilterColumn != "NationalNo")
                // في هذه الحالة نتعامل مع أرقام وليس نص
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام = (مطابقة تامة)
            else // إذا كان العمود نص
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام LIKE (يبدأ بـ)

            lblRecordsCount.Text = _dtAllDrivers.Rows.Count.ToString(); // تحديث عدد السجلات بعد الفلترة
        }

        // حدث الضغط على مفتاح في حقل البحث (للتحكم في المدخلات)
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // نسمح بإدخال أرقام فقط في حالة اختيار "Driver ID" أو "Person ID"
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                // منع إدخال أي حرف ليس رقماً أو حرف تحكم (مثل Backspace)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // حدث النقر على قائمة "Show Details" (عرض التفاصيل)
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // الحصول على رقم الشخص من الصف المحدد (العمود الثاني)
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            // إنشاء وعرض نموذج معلومات الشخص
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog(); // عرض النموذج كحوار
            // تحديث القائمة
            frmListDrivers_Load(null, null);

        }

        // حدث النقر على قائمة "Issue International License" (إصدار رخصة دولية)
        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // عرض رسالة تفيد بأن هذه الميزة لم يتم تنفيذها بعد
            MessageBox.Show("Not implemented yet.");
        }

        // حدث النقر على قائمة "Show Person License History" (عرض سجل رخص الشخص)
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // الحصول على رقم الشخص من الصف المحدد (العمود الثاني)
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;

          
            // إنشاء وعرض نموذج سجل رخص الشخص
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog(); // عرض النموذج كحوار
        }
    }
}
