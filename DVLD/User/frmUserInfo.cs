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
    public partial class frmUserInfo: Form
    {
        private int _UserID; // متغير خاص لتخزين رقم المستخدم

        // Constructor للنموذج - يأخذ رقم المستخدم كمعامل
        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID=UserID; // حفظ رقم المستخدم
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            // تحميل معلومات المستخدم في عنصر التحكم (User Control)
            ctrlUserCard1.LoadUserInfo(_UserID);

        }
    }
}
