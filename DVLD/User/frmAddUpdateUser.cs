using DVLD.Properties; // استيراد مساحة الاسم الخاصة بالموارد (Resources)
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
using DVLD_Buisness; // استيراد طبقة الأعمال (Business Layer) - مكرر
using DVLD.Classes; // استيراد مساحة الاسم الخاصة بالكلاسات العامة
using DVLD.People; // استيراد مساحة الاسم الخاصة بنماذج الأشخاص
using DVLD.Controls; // استيراد مساحة الاسم الخاصة بعناصر التحكم المخصصة
using System.Runtime.Remoting.Messaging; // استيراد مكتبة Remoting.Messaging للرسائل

namespace DVLD.User // مساحة الاسم الخاصة بنماذج المستخدمين
{
    public partial class frmAddUpdateUser: Form // نموذج إضافة/تحديث مستخدم (partial class لأن Designer.cs يحتوي على باقي الكود)
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode; // متغير لتحديد الوضع الحالي (إضافة أو تحديث)
        private int _UserID = -1; // رقم المستخدم (-1 يعني مستخدم جديد)
        clsUser _User; // كائن المستخدم من طبقة الأعمال
       
        // Constructor للنموذج - يُستخدم عند إضافة مستخدم جديد
        public frmAddUpdateUser()
        {
            InitializeComponent(); // تهيئة مكونات النموذج

            _Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة
        }

        // Constructor للنموذج - يُستخدم عند تحديث مستخدم موجود
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent(); // تهيئة مكونات النموذج

            _Mode = enMode.Update; // تعيين الوضع إلى تحديث
            _UserID = UserID; // حفظ رقم المستخدم المراد تحديثه
        }

        // دالة خاصة لإعادة تعيين القيم الافتراضية للنموذج
        private void _ResetDefualtValues()
        {
            // هذه الدالة ستهيئ وتعيد تعيين القيم الافتراضية

            if (_Mode == enMode.AddNew) // إذا كان الوضع إضافة جديدة
            {
                lblTitle.Text = "Add New User"; // تعيين عنوان النموذج
                this.Text = "Add New User"; // تعيين عنوان النافذة
                _User = new clsUser(); // إنشاء كائن مستخدم جديد
             
                tpLoginInfo.Enabled = false; // تعطيل تبويب معلومات تسجيل الدخول (يتم تفعيله بعد اختيار الشخص)
              
                ctrlPersonCardWithFilter1.FilterFocus(); // نقل التركيز إلى عنصر التحكم الخاص بالبحث عن شخص
            }
            else // إذا كان الوضع تحديث
            {
                lblTitle.Text = "Update User"; // تعيين عنوان النموذج
                this.Text = "Update User"; // تعيين عنوان النافذة
             
                tpLoginInfo.Enabled = true; // تفعيل تبويب معلومات تسجيل الدخول
                btnSave.Enabled=true; // تفعيل زر الحفظ
             

            }
            
            // تهيئة الحقول النصية
            txtUserName.Text = ""; // اسم المستخدم
            txtPassword.Text = ""; // كلمة المرور
            txtConfirmPassword.Text = ""; // تأكيد كلمة المرور
            chkIsActive.Checked = true; // تفعيل خيار "نشط" افتراضياً


        }

        // دالة خاصة لتحميل بيانات المستخدم من قاعدة البيانات
        private void _LoadData()
        {

            _User = clsUser.FindByUserID(_UserID); // البحث عن المستخدم باستخدام رقم المستخدم
            ctrlPersonCardWithFilter1.FilterEnabled = false; // تعطيل البحث في عنصر التحكم (لأن الشخص محدد مسبقاً)

            if (_User == null) // إذا لم يتم العثور على المستخدم
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close(); // إغلاق النموذج

                return; // إنهاء الدالة
            }

            // الكود التالي لن يتم تنفيذه إذا لم يتم العثور على المستخدم
            lblUserID.Text = _User.UserID.ToString(); // عرض رقم المستخدم
            txtUserName.Text = _User.UserName; // اسم المستخدم
            txtPassword.Text = _User.Password; // كلمة المرور
            txtConfirmPassword.Text = _User.Password; // تأكيد كلمة المرور (نفس كلمة المرور)
            chkIsActive.Checked = _User.IsActive; // حالة المستخدم (نشط/غير نشط)
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID); // تحميل معلومات الشخص المرتبط بالمستخدم
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues(); // إعادة تعيين القيم الافتراضية

            if (_Mode == enMode.Update) // إذا كان الوضع تحديث
                _LoadData(); // تحميل بيانات المستخدم

        }

        // حدث النقر على زر الحفظ
        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if (!this.ValidateChildren()) // التحقق من صحة جميع الحقول في النموذج
            {
                // هنا لا نتابع لأن النموذج غير صحيح
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // إنهاء الدالة

            }

            // تعيين قيم المستخدم من الحقول
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID; // رقم الشخص
            _User.UserName = txtUserName.Text.Trim(); // اسم المستخدم (مع إزالة المسافات)
            _User.Password = txtPassword.Text.Trim(); // كلمة المرور (مع إزالة المسافات)
            _User.IsActive = chkIsActive.Checked; // حالة المستخدم


            if (_User.Save()) // محاولة حفظ المستخدم
            {
                lblUserID.Text = _User.UserID.ToString(); // عرض رقم المستخدم الجديد
                // تغيير وضع النموذج إلى تحديث
                _Mode = enMode.Update; // تغيير الوضع إلى تحديث
                lblTitle.Text = "Update User"; // تحديث عنوان النموذج
                this.Text = "Update User"; // تحديث عنوان النافذة

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information); // رسالة نجاح
            }
            else // إذا فشل الحفظ
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // رسالة خطأ

        }

        // حدث التحقق من صحة حقل تأكيد كلمة المرور
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim()) // إذا كان تأكيد كلمة المرور لا يطابق كلمة المرور
            {
                    e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                    errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!"); // عرض رسالة خطأ
            }
            else // إذا كان التأكيد صحيحاً
            {
                errorProvider1.SetError(txtConfirmPassword, null); // إزالة رسالة الخطأ
            };

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( string.IsNullOrEmpty ( txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };


            if (_Mode == enMode.AddNew)
            {

                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };
            } 
            else
            {
                //incase update make sure not to use anothers user name
                if (_User.UserName !=txtUserName.Text.Trim())
                {
                        if (clsUser.isUserExist(txtUserName.Text.Trim()))
                        {
                            e.Cancel = true;
                            errorProvider1.SetError(txtUserName, "username is used by another user");
                            return;
                        }
                        else
                        {
                            errorProvider1.SetError(txtUserName, null);
                        };
                }
            }
        }

     
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            } 
               
            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID!=-1)
            {
                
                if (clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                } 
                
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }
            
            else
            
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }

        }


        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
