using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;


namespace ListaPdf
{
    public class List_pdff
    {
        static List<string> lista = new List<string>();
        static string name_list = "Documento", edit_text = string.Empty, new_title = string.Empty, filePath2 = string.Empty, filePath = string.Empty;
        static int edit_position = 0;

        static void Main(string[] args)
        {
            criacao_lista();
        }

        static void criacao_lista()
        {
            Console.Clear();

            string elements, title;
            int cont;

            Console.WriteLine("NETLIST CREATE");
            Console.WriteLine("--------------\n- Para ir ao próximo elemento pressione a tecla ENTER.\n- Para encerrar a criação da lista, editar os elementos adicionados ou salvar,\npressione a tecla ENTER em um novo elemento vazio.");
            Console.Write("--------------\nQual será o título da lista: ");
            title = Console.ReadLine();

            lista.Add($"{title}\n\n");

            if (title != string.Empty)
            {
                name_list = title;
            }

            for (cont = 1; cont >= 0; cont++)
            {
                Console.Write($"\nDigite o {cont}° elemento da lista: ");
                elements = Console.ReadLine();

                if (elements == "")
                {
                    Console.Clear();
                    cont = -999;
                    consultar_lista();
                }

                else
                {
                    lista.Add($"{cont}. {elements}");
                }
            }
        }

        static void consultar_lista()
        {
            Console.Clear();

            int option = 0;

            Console.WriteLine("\nSua lista: ");
            Console.WriteLine("--------------------");

            foreach (string e in lista)
            {
                Console.WriteLine(e);
            }

            try
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("\nSelecione uma opção: \n1 - Salvar Lista\n2 - Editar lista\n3 - Sair");
                Console.Write("\n> ");
                option = int.Parse(Console.ReadLine());
            }

            catch (SystemException)
            {
                Console.WriteLine("Digite uma opção válida.\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                consultar_lista();
            }

            switch (option)
            {
                case 1:
                    salvar_lista();
                    break;

                case 2:
                    editar_lista();
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("\nAplicação encerrada.\n");
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Digite uma opção válida.\nPressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    consultar_lista();
                    break;
            }
        }

        static void editar_lista()

        {
            Console.Clear();

            Console.WriteLine("\nSua lista: ");
            Console.WriteLine("--------------------");

            foreach (string p in lista)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("--------------------");

            verificar_posicao_existe_editar_informacoes();

            Console.WriteLine("\nSua lista: ");
            Console.WriteLine("--------------------");

            if (edit_position == 0 && edit_text != string.Empty)
            {
                lista[edit_position] = edit_text + "\n\n";
                name_list = edit_text;
            }

            else
            {
                lista[edit_position] = edit_position + ". " + edit_text;
            }

            foreach (string f in lista)
            {
                Console.WriteLine(f);
            }

            Console.WriteLine("--------------------");

            consultar_lista();
        }

        static void verificar_posicao_existe_editar_informacoes()
        {
            try
            {
                Console.WriteLine("Digite a posição que deseja alterar(Se for o título, digite 0).");
                Console.WriteLine("Digite -1 se quiser voltar ao menu anterior.");
                Console.Write("\n> ");
                edit_position = int.Parse(Console.ReadLine());
            }

            catch (SystemException)
            {
                Console.WriteLine("\nDigite uma opção válida.\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                consultar_lista();
            }

            if (edit_position >= 0 && edit_position < lista.Count)
            {
                Console.Write("\nDigite o novo texto da posição: ");
                edit_text = Console.ReadLine();
            }

            if (edit_position == -1)
            {
                consultar_lista();
            }

            if (edit_position > lista.Count)
            {
                Console.WriteLine($"\nDigite uma posição válida. Sua lista possue {lista.Count - 1} posições após o título.\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                editar_lista();
            }
        }

        static void salvar_lista()
        {
            Console.Clear();
            int format = 0;

            try
            {
                Console.WriteLine($"\n\nSelecione o formato do arquivo:\n\n1 - Arquivo de texto(.txt)\n2 - PDF");
                Console.Write("\n> ");
                format = int.Parse(Console.ReadLine());
            }

            catch (SystemException)
            {
                Console.WriteLine("\nDigite uma opção válida.\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                salvar_lista();
            }

            switch (format)
            {
                case 1: salvar_txt(); break;
                case 2: salvar_pdf(); break;
                default:
                    Console.WriteLine("\nDigite uma opção válida.\nPressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    salvar_lista();
                    break;
            }
        }

        static void salvar_txt()
        {
            Console.Clear();

            string file_name_txt = $"{name_list}\n\n";

            try
            {
                string folderName = "Arquivos de texto";
                string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = Path.Combine(rootDirectory, folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                filePath = verificar_arquivo_txt_existente(folderPath, file_name_txt);
                filePath = Path.Combine($"{filePath}.txt");

                File.WriteAllLines(filePath, lista);
            }

            catch (SystemException)
            {
                Console.WriteLine("\nAlgo de errado aconteceu.\nPressione qualquer tecla para voltar.");
                Console.ReadKey();
                consultar_lista();
            }

            Console.WriteLine($"\nLista salva com sucesso em: {filePath}");

            opcao_fazer_nova_lista();
        }

        static void salvar_pdf()
        {
            Console.Clear();

            string file_name_pdf = name_list;

            lista.RemoveAt(0);//limpa o titulo adicionado anteriormente, ja que vamos adicionar um novo titulo personalizado

            PdfFont title_font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont text_font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Style style_title = new Style()
            .SetFont(title_font)
            .SetFontSize(16);

            Style style_text = new Style()
            .SetFont(text_font)
            .SetFontSize(12);

            try
            {
                string folderName2 = "Arquivos PDF";
                string rootDirectory2 = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath2 = Path.Combine(rootDirectory2, folderName2);

                if (!Directory.Exists(folderPath2))
                {
                    Directory.CreateDirectory(folderPath2);
                }

                filePath2 = verificar_arquivo_pdf_existente(file_name_pdf, folderPath2);

                using var document = new Document(new PdfDocument(new PdfWriter($"{filePath2}.pdf")));

                Paragraph title = new Paragraph().Add(new Text(file_name_pdf).AddStyle(style_title));
                document.Add(title);

                foreach (string elementos in lista)
                {
                    document.Add(new Paragraph().Add(new Text(elementos).AddStyle(style_text)));
                }

                document.Close();
            }

            catch (SystemException)
            {
                Console.WriteLine("\nAlgo de errado aconteceu.\nPressione qualquer tecla para voltar.");
                Console.ReadKey();
                consultar_lista();
            }

            Console.WriteLine($"\nLista salva com sucesso em: {filePath2}");
            opcao_fazer_nova_lista();
        }

        static string verificar_arquivo_pdf_existente(string file_name_pdf, string folderPath2)
        {
            int cont = 1;
            string originalFilePath_pdf = Path.Combine(folderPath2, file_name_pdf);
            string newFilePath_pdf = originalFilePath_pdf;

            while (File.Exists($"{newFilePath_pdf}.pdf"))
            {
                newFilePath_pdf = Path.Combine(folderPath2, $"{file_name_pdf}({cont++})");
            }

            return newFilePath_pdf;
        }

        static string verificar_arquivo_txt_existente(string folderPath, string file_name_txt)
        {
            int cont = 1;
            string originalFilePath_txt = Path.Combine(folderPath, file_name_txt);
            string newFilePath_txt = originalFilePath_txt;

            while (File.Exists($"{newFilePath_txt}.txt"))
            {
                newFilePath_txt = Path.Combine(folderPath, $"{file_name_txt}({cont++})");
            }

            return newFilePath_txt;
        }

        static void opcao_fazer_nova_lista()
        {
            int option = 0;

            try
            {
                Console.WriteLine("\nSelecione uma opção:\n\n1 - Sair\n2 - Fazer nova lista");
                Console.Write("\n> ");
                option = int.Parse(Console.ReadLine());
            }


            catch (SystemException)
            {
                Console.WriteLine("\nDigite uma opção válida.\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                opcao_fazer_nova_lista();
            }

            switch (option)
            {
                case 1:
                    Console.WriteLine("Aplicação encerrada.");
                    System.Environment.Exit(0);
                    break;

                case 2:
                    lista.Clear();
                    criacao_lista();
                    break;

                default:
                    Console.WriteLine("\nDigite uma opção válida.\nPressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    salvar_lista();
                    break;
            }
        }
    }
}