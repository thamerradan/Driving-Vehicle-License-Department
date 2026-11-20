using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Text.RegularExpressions; // استيراد مكتبة RegularExpressions للتعامل مع التعبيرات النمطية
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة

namespace DVLD.Classes // مساحة الاسم الخاصة بالكلاسات العامة
{
    public class clsValidatoin // كلاس للتحقق من صحة البيانات (Validation)
    {

        // دالة ثابتة للتحقق من صحة عنوان البريد الإلكتروني
        // ترجع true إذا كان العنوان صحيحاً، و false إذا كان غير صحيح
        public static bool ValidateEmail(string emailAddress)
        {
            // نمط (Pattern) للتحقق من صحة البريد الإلكتروني
            // يتحقق من وجود @ واسم النطاق
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern); // إنشاء كائن Regex للنمط

            return regex.IsMatch(emailAddress); // التحقق من تطابق العنوان مع النمط
        }

        // دالة ثابتة للتحقق من أن النص يحتوي على أرقام صحيحة فقط
        // ترجع true إذا كان النص يحتوي على أرقام فقط، و false إذا كان يحتوي على أحرف أخرى
        public static bool ValidateInteger(string Number)
        {
            // نمط للتحقق من أن النص يحتوي على أرقام فقط (0-9)
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern); // إنشاء كائن Regex

            return regex.IsMatch(Number); // التحقق من تطابق النص مع النمط
        }

        // دالة ثابتة للتحقق من أن النص يحتوي على رقم عشري (float)
        // ترجع true إذا كان النص يحتوي على رقم عشري صحيح، و false إذا كان غير صحيح
        public static bool ValidateFloat(string Number)
        {
            // نمط للتحقق من أن النص يحتوي على رقم عشري (يمكن أن يحتوي على نقطة عشرية)
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern); // إنشاء كائن Regex

            return regex.IsMatch(Number); // التحقق من تطابق النص مع النمط
        }

        // دالة ثابتة للتحقق من أن النص يحتوي على رقم (صحيح أو عشري)
        // ترجع true إذا كان النص يحتوي على رقم، و false إذا كان لا يحتوي على رقم
        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number)); // التحقق من أن النص رقم صحيح أو عشري
        }


    }
}
