using DVLD.Applications; // استيراد مساحة الاسم الخاصة بطلبات التطبيق
using DVLD.Applications.Detain_License; // استيراد مساحة الاسم الخاصة بطلبات احتجاز الرخص
using DVLD.Applications.International_License; // استيراد مساحة الاسم الخاصة بطلبات الرخص الدولية
using DVLD.Applications.ReplaceLostOrDamagedLicense; // استيراد مساحة الاسم الخاصة بطلبات استبدال الرخص المفقودة أو التالفة
using DVLD.Applications.Rlease_Detained_License; // استيراد مساحة الاسم الخاصة بطلبات إطلاق الرخص المحتجزة
using DVLD.Classes; // استيراد مساحة الاسم الخاصة بالكلاسات العامة
using DVLD.Drivers; // استيراد مساحة الاسم الخاصة بالسائقين
using DVLD.Licenses; // استيراد مساحة الاسم الخاصة بالرخص
using DVLD.Licenses.International_License; // استيراد مساحة الاسم الخاصة بالرخص الدولية
using DVLD.Login; // استيراد مساحة الاسم الخاصة بنموذج تسجيل الدخول
using DVLD.People; // استيراد مساحة الاسم الخاصة بالأشخاص
using DVLD.Tests; // استيراد مساحة الاسم الخاصة بالاختبارات
using DVLD.User; // استيراد مساحة الاسم الخاصة بالمستخدمين
using System; // استيراد مكتبة System الأساسية
using System.Drawing; // استيراد مكتبة Drawing للتعامل مع الرسومات
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية


namespace DVLD // مساحة الاسم الرئيسية للتطبيق
{

    public partial class frmMain : Form // النموذج الرئيسي للتطبيق (partial class لأن Designer.cs يحتوي على باقي الكود)
    {
        frmLogin _frmLogin; // متغير لتخزين مرجع لنموذج تسجيل الدخول

        // Constructor للنموذج الرئيسي
        // يأخذ نموذج تسجيل الدخول كمعامل لإمكانية إظهاره مرة أخرى عند تسجيل الخروج
        public frmMain( frmLogin frm )
        {
            InitializeComponent(); // تهيئة مكونات النموذج (يتم إنشاؤها تلقائياً من Designer)
            _frmLogin= frm; // حفظ مرجع لنموذج تسجيل الدخول

        }

        // حدث النقر على قائمة "Local License" (رخصة قيادة محلية)
        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج إضافة/تحديث طلب رخصة قيادة محلية
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog(); // عرض النموذج كحوار (modal)
        }

        // حدث النقر على قائمة "People" (الأشخاص)
        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج قائمة الأشخاص
            Form frm = new frmListPeople();
            frm.ShowDialog(); // عرض النموذج كحوار
        }

        // حدث النقر على قائمة "Employees" (الموظفين/المستخدمين)
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج قائمة المستخدمين
            Form frm = new frmListUsers();
            frm.ShowDialog(); // عرض النموذج كحوار
        }

        // حدث تحميل النموذج الرئيسي (يتم تنفيذه عند فتح النموذج)
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White; // تعيين لون الخلفية إلى الأبيض
            lblLoggedInUser.Text = "LoggedIn User: " + clsGlobal.CurrentUser.UserName; // عرض اسم المستخدم المسجل دخوله
            this.Refresh(); // تحديث النموذج لعرض التغييرات

        }

        // حدث النقر على قائمة "Current User Info" (معلومات المستخدم الحالي)
        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج معلومات المستخدم الحالي
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog(); // عرض النموذج كحوار

        }

        // حدث النقر على قائمة "Sign Out" (تسجيل الخروج)
        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null; // مسح معلومات المستخدم الحالي
            _frmLogin.Show(); // إظهار نموذج تسجيل الدخول مرة أخرى
            this.Close(); // إغلاق النموذج الرئيسي
        }

        // حدث النقر على قائمة "Change Password" (تغيير كلمة المرور)
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج تغيير كلمة المرور
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog(); // عرض النموذج كحوار

        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();

        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicenseApplication frm = new frmRenewLocalDrivingLicenseApplication();
            frm.ShowDialog();

        }

       

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }

      
        private void vehiclesLicensesServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void manageLocalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();

        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();

        }

      

        private void ManageInternationaDrivingLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListInternationalLicesnseApplications frm = new frmListInternationalLicesnseApplications();
            frm.ShowDialog();

        }

        private void ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicenseApplication frm = new frmReplaceLostOrDamagedLicenseApplication();
            frm.ShowDialog();

        }

        private void ManageDetainedLicensestoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses frm = new frmListDetainedLicenses();
            frm.ShowDialog();

        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
           frmDetainLicenseApplication frm = new frmDetainLicenseApplication();
            frm.ShowDialog();

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm= new frmReleaseDetainedLicenseApplication();   
            frm.ShowDialog();

        }
    }
}
