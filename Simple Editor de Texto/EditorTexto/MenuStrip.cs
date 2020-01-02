using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace EditorTexto
{
    // Permite sobreescribir los colores para el SubMenu
    class ColorsMenuStrip : ProfessionalColorTable
    {
        //Color comun
        Color CommonColor = Color.Transparent;
   
        //Color para el hover
        Color HoverBackground = Color.FromArgb(253,236,251);
        //Color por defecto
        Color MenuBackground = Color.FromArgb(8,105,185);

        #region
        //Colores por defecto
        public override Color MenuItemPressedGradientBegin
        {
            get{return CommonColor;}
        }
        public override Color MenuItemPressedGradientEnd
        {
            get{ return CommonColor; }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return CommonColor; }
           
        }
        public override Color ButtonSelectedHighlight
        {
            get { return CommonColor; }

        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return CommonColor; }

        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return CommonColor; }

        }
        public override Color MenuBorder
        {
            get { return CommonColor; }


        }
        public override Color MenuItemBorder
        {
            get { return CommonColor; }

        }
   
        //color para el  submenu
        public override Color ToolStripDropDownBackground
        {
            get { return Color.White; }
        }
        //color para el hover submenu
        public override Color MenuItemSelected
        {
            get { return HoverBackground; }
        }
        #endregion
    }
   
   }
