using DVLD.Classes; // استيراد مساحة الاسم الخاصة بالكلاسات العامة
using DVLD_Buisness; // استيراد طبقة الأعمال (Business Layer)
using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.ComponentModel; // استيراد مكتبة ComponentModel للخصائص والسمات
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Drawing; // استيراد مكتبة Drawing للتعامل مع الرسومات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية

namespace DVLD.Login // مساحة الاسم الخاصة بنموذج تسجيل الدخول
{
    public partial class frmLogin : Form // نموذج تسجيل الدخول (partial class لأن Designer.cs يحتوي على باقي الكود)
    {
        // Constructor للنموذج
        public frmLogin()
        {
            InitializeComponent(); // تهيئة مكونات النموذج (يتم إنشاؤها تلقائياً من Designer)
        }

       
        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج
        }

        // حدث النقر على زر تسجيل الدخول
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // البحث عن مستخدم باستخدام اسم المستخدم وكلمة المرور
            // Trim() لإزالة المسافات من البداية والنهاية
            clsUser user= clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(),txtPassword.Text.Trim());

            if (user != null) // إذا تم العثور على المستخدم
            { 

                if (chkRememberMe.Checked ) // إذا كان خيار "تذكرني" مفعلاً
                {
                    // حفظ اسم المستخدم وكلمة المرور في ملف
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                } 
                  else // إذا كان خيار "تذكرني" غير مفعّل
                {
                    // حفظ اسم مستخدم وكلمة مرور فارغة (لحذف البيانات المحفوظة)
                    clsGlobal.RememberUsernameAndPassword("", "");

                }

                // في حالة أن المستخدم غير نشط
                if (!user.IsActive )
                {

                    txtUserName.Focus(); // نقل التركيز إلى حقل اسم المستخدم
                    // عرض رسالة خطأ تفيد بأن الحساب غير نشط
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // إنهاء الدالة (عدم المتابعة)
                }

                 clsGlobal.CurrentUser = user; // حفظ المستخدم الحالي في المتغير العام
                 this.Hide(); // إخفاء نموذج تسجيل الدخول
                 frmMain frm = new frmMain(this); // إنشاء النموذج الرئيسي (نمرر this لإمكانية إظهار نموذج تسجيل الدخول مرة أخرى)
                 frm.ShowDialog(); // عرض النموذج الرئيسي كحوار


            } else // إذا لم يتم العثور على المستخدم (بيانات الدخول خاطئة)
            {
                txtUserName.Focus(); // نقل التركيز إلى حقل اسم المستخدم
                // عرض رسالة خطأ تفيد بأن بيانات الدخول غير صحيحة
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    

        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = ""; // متغيرات لتخزين بيانات الدخول المحفوظة

            // محاولة الحصول على بيانات الدخول المحفوظة
            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName; // تعبئة حقل اسم المستخدم بالبيانات المحفوظة
                txtPassword.Text = Password; // تعبئة حقل كلمة المرور بالبيانات المحفوظة
                chkRememberMe.Checked = true; // تفعيل خيار "تذكرني"
            }
            else // إذا لم تكن هناك بيانات محفوظة
                chkRememberMe.Checked = false; // تعطيل خيار "تذكرني"

        }
    }
}
