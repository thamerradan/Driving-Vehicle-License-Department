using DVLD.Applications; // استيراد مساحة الاسم الخاصة بطلبات التطبيق
using DVLD.Login; // استيراد مساحة الاسم الخاصة بنموذج تسجيل الدخول
using DVLD.Tests; // استيراد مساحة الاسم الخاصة بالاختبارات
using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية

namespace DVLD // مساحة الاسم الرئيسية للتطبيق
{
    internal static class Program // كلاس ثابت يحتوي على نقطة الدخول الرئيسية للتطبيق
    {
        /// <summary>
        /// The main entry point for the application.
        /// نقطة الدخول الرئيسية للتطبيق
        /// </summary>
        [STAThread] // سمة تشير إلى أن التطبيق يستخدم Single Threaded Apartment (مطلوب لـ Windows Forms)
        static void Main() // الدالة الرئيسية التي يتم تنفيذها عند بدء تشغيل التطبيق
        {
            Application.EnableVisualStyles(); // تفعيل الأنماط البصرية الحديثة لويندوز
            Application.SetCompatibleTextRenderingDefault(false); // تعطيل عرض النص المتوافق مع .NET Framework القديم
            // Application.Run(new frmMain()); // تشغيل النموذج الرئيسي (معطل حالياً)
          // Application.Run(new frmTest2()); // تشغيل نموذج اختبار (معطل حالياً)
          Application.Run(new frmLogin()); // تشغيل نموذج تسجيل الدخول (النموذج الذي يبدأ به التطبيق)
         


        }
    }
}
