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

namespace DVLD.User
{
    public partial class frmListUsers : Form
    {
        private static DataTable _dtAllUsers ;

        // Constructor للنموذج
        public frmListUsers()
        {
            InitializeComponent();
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmListUsers_Load(object sender, EventArgs e)
        {
            // الحصول على جميع المستخدمين من قاعدة البيانات
            _dtAllUsers = clsUser.GetAllUsers();
            // ربط جدول البيانات بالشبكة (DataGridView)
            dgvUsers.DataSource = _dtAllUsers;
            // تعيين الفلتر الافتراضي (الخيار الأول)
            cbFilterBy.SelectedIndex = 0;
            // عرض عدد السجلات
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

            // تخصيص أعمدة الشبكة (العناوين والعرض)
            dgvUsers.Columns[0].HeaderText = "User ID"; // عنوان العمود الأول
            dgvUsers.Columns[0].Width = 110; // عرض العمود الأول

            dgvUsers.Columns[1].HeaderText = "Person ID"; // عنوان العمود الثاني
            dgvUsers.Columns[1].Width = 120; // عرض العمود الثاني

            dgvUsers.Columns[2].HeaderText = "Full Name"; // عنوان العمود الثالث
            dgvUsers.Columns[2].Width = 350; // عرض العمود الثالث

            dgvUsers.Columns[3].HeaderText = "UserName"; // عنوان العمود الرابع
            dgvUsers.Columns[3].Width = 120; // عرض العمود الرابع

            dgvUsers.Columns[4].HeaderText = "Is Active"; // عنوان العمود الخامس
            dgvUsers.Columns[4].Width = 120; // عرض العمود الخامس

           
        }

        // حدث تغيير اختيار نوع الفلتر
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            // إذا كان الفلتر المحدد هو "Is Active" (نشط/غير نشط)
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible= false; // إخفاء حقل البحث النصي
                cbIsActive.Visible = true; // إظهار ComboBox للاختيار بين نعم/لا/الكل
                cbIsActive.Focus(); // نقل التركيز إلى ComboBox
                cbIsActive.SelectedIndex = 0; // تعيين الخيار الافتراضي (الخيار الأول)
            } 
            
            else // إذا كان الفلتر نوع آخر

            {
                // إظهار حقل البحث النصي إذا كان الفلتر ليس "None"
                txtFilterValue.Visible = (cbFilterBy.Text !="None") ;
                cbIsActive.Visible = false; // إخفاء ComboBox

                if (cbFilterBy.Text == "None") // إذا كان الفلتر "None"
                {
                    txtFilterValue.Enabled = false; // تعطيل حقل البحث
                }
                else // إذا كان الفلتر نوع آخر
                    txtFilterValue.Enabled = true; // تفعيل حقل البحث

                txtFilterValue.Text = ""; // مسح نص البحث
                txtFilterValue.Focus(); // نقل التركيز إلى حقل البحث
            }

           
        }

        // حدث تغيير نص حقل البحث (Filter)
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = ""; // متغير لتخزين اسم العمود المراد الفلترة عليه
            // ربط الفلتر المحدد باسم العمود الحقيقي في قاعدة البيانات
            switch (cbFilterBy.Text)
            {
                case "User ID": // إذا كان الفلتر "User ID"
                    FilterColumn = "UserID"; // اسم العمود الحقيقي
                    break;
                case "UserName": // إذا كان الفلتر "UserName"
                    FilterColumn = "UserName"; // اسم العمود الحقيقي
                    break;

                case "Person ID": // إذا كان الفلتر "Person ID"
                    FilterColumn = "PersonID"; // اسم العمود الحقيقي
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
                _dtAllUsers.DefaultView.RowFilter = ""; // إزالة الفلتر (عرض جميع السجلات)
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString(); // تحديث عدد السجلات
                return; // إنهاء الدالة
            }


            // إذا كان العمود رقم (ليس نص)
            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                // في هذه الحالة نتعامل مع أرقام وليس نص
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام = (مطابقة تامة)
            else // إذا كان العمود نص
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim()); // فلترة باستخدام LIKE (يبدأ بـ)

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString(); // تحديث عدد السجلات بعد الفلترة
        }

        // حدث تغيير اختيار حالة المستخدم (نشط/غير نشط/الكل)
        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

             
          string FilterColumn = "IsActive"; // اسم العمود المراد الفلترة عليه
          string FilterValue =cbIsActive.Text; // القيمة المحددة من ComboBox

            // تحويل القيمة النصية إلى قيمة رقمية
            switch (FilterValue)
            {
                case "All": // إذا كان الاختيار "الكل"
                    break; // لا تغيير
                case "Yes": // إذا كان الاختيار "نعم" (نشط)
                    FilterValue = "1"; // تحويل إلى 1
                    break;
                case "No": // إذا كان الاختيار "لا" (غير نشط)
                    FilterValue = "0"; // تحويل إلى 0
                    break;
            }


            if (FilterValue == "All") // إذا كان الاختيار "الكل"
                _dtAllUsers.DefaultView.RowFilter = ""; // إزالة الفلتر (عرض جميع السجلات)
            else // إذا كان الاختيار "نعم" أو "لا"
                // في هذه الحالة نتعامل مع أرقام وليس نص
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue); // فلترة باستخدام = (مطابقة تامة)

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString(); // تحديث عدد السجلات بعد الفلترة


        }

        // حدث النقر على زر إضافة مستخدم
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج إضافة مستخدم جديد
            frmAddUpdateUser Frm1 = new frmAddUpdateUser ();
            Frm1.ShowDialog(); // عرض النموذج كحوار
            frmListUsers_Load(null, null); // إعادة تحميل قائمة المستخدمين بعد الإغلاق
        }

        // حدث النقر على قائمة "Edit" (تعديل)
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // إنشاء وعرض نموذج تحديث المستخدم المحدد
            // الحصول على رقم المستخدم من الصف المحدد في الشبكة
            frmAddUpdateUser Frm1 = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog(); // عرض النموذج كحوار
            frmListUsers_Load(null, null); // إعادة تحميل قائمة المستخدمين بعد الإغلاق

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser Frm1 = new frmAddUpdateUser();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);

        }

        // حدث النقر المزدوج على خلية في الشبكة
        private void dgvUsers_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // إنشاء وعرض نموذج معلومات المستخدم المحدد
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog(); // عرض النموذج كحوار
           
        }

        // حدث النقر على قائمة "Show Details" (عرض التفاصيل)
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج معلومات المستخدم المحدد
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog(); // عرض النموذج كحوار
           
        }

        // حدث النقر على قائمة "Change Password" (تغيير كلمة المرور)
        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {

            // الحصول على رقم المستخدم من الصف المحدد
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            // إنشاء وعرض نموذج تغيير كلمة المرور
            frmChangePassword Frm1 = new frmChangePassword(UserID);
            Frm1.ShowDialog(); // عرض النموذج كحوار

        }

        // حدث الضغط على مفتاح في حقل البحث (للتحكم في المدخلات)
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // نسمح بإدخال أرقام فقط في حالة اختيار "Person ID" أو "User ID"
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                // منع إدخال أي حرف ليس رقماً أو حرف تحكم (مثل Backspace)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // حدث النقر على قائمة "Delete" (حذف)
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // الحصول على رقم المستخدم من الصف المحدد
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            // محاولة حذف المستخدم
            if (clsUser.DeleteUser(UserID))
            {
                // إذا نجح الحذف، عرض رسالة نجاح
                MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmListUsers_Load(null, null); // إعادة تحميل قائمة المستخدمين
            }

            else // إذا فشل الحذف
                // عرض رسالة خطأ تفيد بأن المستخدم لم يُحذف بسبب وجود بيانات مرتبطة به
                MessageBox.Show("User is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);


            


        }
    }
}
