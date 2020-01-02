using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace EditorTexto
{


    public partial class EditText : Form
    {
        private bool Move;
        private Point Position;
        static FileOperations Fs = new FileOperations();

        public EditText()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Se realiza un render para el Menustrip y sobreescriba los colores por defecto
            Opciones.Renderer = new ToolStripProfessionalRenderer(new ColorsMenuStrip());
            //borra el recuadro de la imagen por defecto
            // de MenuStrip
            foreach (ToolStripMenuItem menuItem in Opciones.Items)
                ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = false;
            //cargar las fuentes disponibles
            foreach (var fonts in FontFamily.Families)
            {
                Fonts_load.Items.Add(fonts.Name);
            }
            //Carga los numeros disponibles para el texto
            for (int i = 7; i <= 30; i++)
                NumberFont.Items.Add(i.ToString());

        }
        //Eventos permitiran mover el formulario sin bordes
        private void MoveFormMouseDown(object sender, MouseEventArgs e)
        {
            // Verifica que solo sea el click izquierdo el que pueda mover el formulario
            if (e.Button == MouseButtons.Left)
            {
                Move = true;
                Position = new Point(e.X, e.Y);
            }
            else
                Move = false;
   
        }
        // Permite mover la ventana
        private void MoveFormMouseMove(object sender, MouseEventArgs e)
        {
            if (Move)
            {

                // Se establece la nueva posicion de la ventana 
                Point newPoint= PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(-Position.X, -Position.Y);

                Location = newPoint;
            }
        }
       
       // Establece un valor falso si se suelta el boton del raton
        private void MoveFormMouseUp(object sender, MouseEventArgs e)
        {
            Move = false;
        }


        // Establece cuantas palabras encuentra en el texto
        private int TotalWords(string Text)
        {
            // se crea el patron para validar la expresion regulas
            string pattern = @"\b[A-Za-z]";
            Regex validation = new Regex(pattern);
            // Recupera cuantas veces aparece el patron en el texto
            int count = validation.Matches(Text).Count;
            return count;


        }
        //Obtiene el total de palabras en el texto
        private int TWords() => (TotalWords(TextContent.Text));
        private int TWords(string text) => (TotalWords(text));
        //modifica el tipo de fuente seleccionado
        private void ModifyTextRichText(FontStyle fontX)
        {
            TextContent.SelectionFont = new Font(TextContent.SelectionFont.FontFamily,
                   TextContent.Font.Size, fontX);

        }
        //Resumen
        //Determina las posiciones de las coincidencias del texto buscado en el RichText
        private void MovePositionStringSearch(string textSearch)
        {
           
            int at = 0;
            int indexStar = 0;
            while (true)
            {
                //determina la posicion que se encuenta la coincidencia del texto
                at = TextContent.Text.IndexOf(textSearch, indexStar);
                // si IndexOf devuelve -1 no encontro nada, termina
                if (at == -1) break;
                // Determina la posicion siguiente que va a empezar a buscar
                
                indexStar = at + textSearch.Length;
                // Determina el punto en donde se va a empezar a colorear
                TextContent.SelectionStart = at;
                // Determina cuantas posiciones tiene que recorrer para colorear
                TextContent.SelectionLength = textSearch.Length;
                //colore
                TextContent.SelectionBackColor = Color.Bisque;
            }
        }
        //Establece la pagina que se va a imprimir 
        private void Print_Document(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //captura el texto por defecto
            Font FontRichTextDefaul = new Font(TextContent.Font.FontFamily,
               TextContent.Font.Size);
            //Se establece la forma que se va a imprimir la pagina
            e.Graphics.DrawString(TextContent.Text, FontRichTextDefaul,
            System.Drawing.Brushes.Black, 0, 0);

        }




        // permite la apertura del archivo
        private void OpenFileSubMenuBtn(object sender, EventArgs e)
        {
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                //Obtiene el nombre del archivo
                NameFilePath.Text = Path.GetFileName(OpenDialog.FileName);
                // se establece la ruta de acceso
                Fs.Path = OpenDialog.FileName;
                TextContent.Text = " ";
                // obtiene el texto del archivo
                string GetTextFile = Fs.ReadFile();
                //coloca el archivo en el RichText
                TextContent.Text = GetTextFile;
                //Obtiene el total de palabras encontradas
                Words.Text = TWords(GetTextFile).ToString();


            }
        }
        //guarda el archivo
        private void SaveFileBtn(object sender, EventArgs e)
        {
            //verifica mediante una variable bandera si el archivo ya se ha abrio
            if (Fs.Flag)
                //Escribe en el archivo.
                Fs.WriteFile(TextContent.Text);
            else
                MessageBox.Show("No contienes, ningun archivo cargado",
                    "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
        //Determina el tipo de fuente(RichText) mediante el evento OnChange de Combobox
        private void SelectTypeFontCombobox(object sender, EventArgs e)
        {

            if (TextContent.SelectedText != " ")
                TextContent.SelectionFont = new Font(Fonts_load.Text,
                     TextContent.Font.Size);
        }
        //Determina el tamaño de fuente(RichText) mediante el evento OnChange de Combobox
        private void SelectedNumberFontCombobox(object sender, EventArgs e)
        {
            if (TextContent.SelectedText != " ")
                TextContent.SelectionFont = new Font(TextContent.Font.FontFamily,
                    Int32.Parse(NumberFont.Text));
        }
        //Determina el color para el texto seleccionado
        private void SelectColor(object sender, EventArgs e)
        {

            if (colorDialog.ShowDialog() == DialogResult.OK)
                TextContent.SelectionColor = colorDialog.Color;

        }
        //Determina el total de palabras mediante el evento KeyPress de RichText
        private void TotalWords(object sender, KeyPressEventArgs e)
        {
            // Si se preciona la tecla de espacio determina cuantas palabras exiten
            if (e.KeyChar == 32)
                Words.Text = TWords().ToString();

        }

        //Determina el total de palabras mediante el evento KeyUp de RichText
        private void TotalWordsKeyUp(object sender, KeyEventArgs e)
        {
            // Si se preciona la tecla de  backspace determina cuantas palabras exiten
            if (e.KeyCode == Keys.Back)
                Words.Text = TWords().ToString();
        }

       //Justifica el texto a la izquierda
        private void AlignmentJustify(object sender, EventArgs e)
        {
            
            TextContent.SelectionAlignment = HorizontalAlignment.Left;
        }
        //Justifica el texto a la derecha

        private void AlignmentRigth(object sender, EventArgs e)
        {
            TextContent.SelectionAlignment = HorizontalAlignment.Right;
        }
        //Justifica el texto al centro
        private void AlignmentCenter(object sender, EventArgs e)
        {
            TextContent.SelectionAlignment = HorizontalAlignment.Center;
        }
        //Justifica el texto a la Izquierda

        private void AlignmentLeft(object sender, EventArgs e)
        {
            TextContent.SelectionAlignment = HorizontalAlignment.Left;
        }
        //Establece viñetas para el texto seleccionado
        private void Bullets(object sender, EventArgs e)
        {
            TextContent.SelectionBullet = true;
        }
        //Establece,  texto underline 
        private void TextUnderlineEvent(object sender, EventArgs e)
        {
            if (!TextContent.SelectionFont.Underline)
                ModifyTextRichText(FontStyle.Underline);
            else
                ModifyTextRichText(FontStyle.Regular);
        }
        //Establece el texto de forma cursiva
        private void TextCursiveEvent(object sender, EventArgs e)
        {
            if (!TextContent.SelectionFont.Italic)
                ModifyTextRichText(FontStyle.Italic);
            else
                ModifyTextRichText(FontStyle.Regular);



        }
        //Establece el texto de forma bold
        private void TextBEvent(object sender, EventArgs e)
        {
            if(!TextContent.SelectionFont.Bold)
                ModifyTextRichText(FontStyle.Bold);
            else
                ModifyTextRichText(FontStyle.Regular);

        }
        //Establece el texto tachado
        private void StrikethroughTextEvent(object sender, EventArgs e)
        {
            if (!TextContent.SelectionFont.Strikeout)
                ModifyTextRichText(FontStyle.Strikeout);
            else
                ModifyTextRichText(FontStyle.Regular);

        }
        //Establece el color de fondo del texto subrayado
        private void SelectionColor(object sender, EventArgs e)
        {
            if(colorDialog.ShowDialog() == DialogResult.OK)
            {
                TextContent.SelectionBackColor = colorDialog.Color;

            }
        }
        //Copia el texto del RichText
        private void CopyTextBtn(object sender, EventArgs e)
        {
            if(TextContent.Text != " ")
            {
                TextContent.Copy();
            }
        }
        //Pega el texto del RichText
        private void PasteTextBtn(object sender, EventArgs e)
        {
            if (TextContent.Text != " ")
                TextContent.Paste();
        }
        //imprime el documento
        private void PrintDocument(object sender, EventArgs e)
        {
           
          //verifica si exito texto y si se a cargado algun archivo
        
            if ((this.TWords() > 0) && Fs.Flag)
            {
                //Se establece el nombre del documento
                printDocument1.DocumentName = Path.GetFileName(Fs.Path);
                //Establece documento que se va a imprimir
                printDialog1.Document = printDocument1;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                    printDocument1.Print();
            }
            else
                MessageBox.Show("¡Datos incorrectos(No tienes archivo cargando, " +
                    "\n o esta vacio el campo de" +
                    " texto)!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

        }

        private void ViewPrintDocument(object sender, EventArgs e)
        {
            //verifica si exito texto y si se a cargado algun archivo
            if ((this.TWords() > 0) && (Fs.Flag))
            {
                //Establece el documento a imprimir
                printPreviewDialog1.Document = printDocument1;
                //Muestra el documento
                printPreviewDialog1.ShowDialog();
            }
            else
                MessageBox.Show("¡Datos incorrectos(No tienes archivo cargando, " +
                    "\n o esta vacio el campo de" +
                    " texto)!", "Error",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

        }
      


      
        //Busca las palabras que coincidan con el texto, cada que se presiona
        //Enter busca la palabra y la subraya
        private void SearchWords(object sender, KeyPressEventArgs e)
        {
            //Verifica si se presiono la tecla de enter y si el campo no esta vacio
            if (e.KeyChar == 13 && InputSearchRich.Text.Length > 0)
                 MovePositionStringSearch(InputSearchRich.Text);


        }
        //Borrra el todo el contenido del RichText
        private void DeleteText_Click(object sender, EventArgs e)
        {
            TextContent.Clear();
        }
       
        //Permite cerrar la ventana
        private void ExitBtnClick(object sender, EventArgs e)
        {
            DialogResult Message = MessageBox.Show(
              "¿Deseas cerrar la aplicacion ?", "Advertencia", MessageBoxButtons.YesNo
              , MessageBoxIcon.Warning);
            if (Message == DialogResult.Yes)
            {
                Application.Exit();
            }

        }
        //Minimiza la ventana
        private void buttMimizedBtnClick(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        //Cierra la aplicacion 
        private void ExitAppSubMenu(object sender, EventArgs e) => Application.Exit();
    }
}
