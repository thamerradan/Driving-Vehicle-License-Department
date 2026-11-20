using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Text.RegularExpressions; // استيراد مكتبة RegularExpressions للتعامل مع التعبيرات النمطية
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة

namespace DVLD.Classes // مساحة الاسم الخاصة بالكلاسات العامة
{
    public class clsFormat // كلاس لتنسيق البيانات (Formatting)
    {
        // دالة ثابتة لتحويل التاريخ إلى صيغة قصيرة
        // ترجع التاريخ بصيغة "dd/MMM/yyyy" (مثال: 25/Jan/2024)
        public static string DateToShort(DateTime Dt1)
        {
            
            return Dt1.ToString("dd/MMM/yyyy"); // تحويل التاريخ إلى سلسلة نصية بالصيغة المحددة
        } 

    }
}
