using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace File_vokab
{	
	class MainClass
	{
        public static List<string> CreateCol(string fst) // функция преобразующая строку в список
        {
            List<string> kol = new List<string>(fst.Split(' '));  //разбивка строки на слова и запись в коллекцию				
            string znak = ".,;!?-:\"()";
            for (int tt = 0; tt < kol.Count; tt++)  // перебор слов в строке
            {
                string p = kol[tt];			// p присваивается значение слова из массива
                char[] buk = p.ToCharArray();
                int k = buk.Length-1;
                if (znak.Contains(buk[k]))		// проверка является ли последний символ в слове знаком препинания							
                {
                    string zn = Convert.ToString(buk[k]);
                    p = p.Remove(k);				// отрезаем знак от слова
                    kol[tt] = p;					//запись нового слова
                    tt++;
                    kol.Insert(tt, zn);			//запись знака в коллекци, дабы его не потерять																								
                }
            }
            return kol;
        }

        public static string newSt(List<string> col, string[] readVoc)  //функция записывает список с изменениями в строку
        {
            string znak = ".,;!?-:\"\"";
            string resultSt = "";
            for (int tt = 0; tt < col.Count; tt++)		//перебор значения коллекции
            {
                foreach (string vk in readVoc)		//перебор  слов из словаря для проверки соответствия
                {
                    string t = vk.Trim();				// удаление знака пробела в начале и в конце строки
                    if (String.Equals(t, col[tt], StringComparison.CurrentCultureIgnoreCase))	//в случае совпадения со словом из словаря (без учета реестра), делает его жирным и курсивом
                        col[tt] = "<b><i>" + col[tt] + "</b></i>";
                }
                if (znak.Contains(col[tt]) == false) resultSt += " ";
                resultSt += col[tt];             //запись слов в строку результат
            }
           return resultSt;
        }

        public static Boolean FileBig(string file)      //проверка превышает ли файл 2Мб
        {
            FileInfo namefile = new FileInfo(file);
            long mb = 1048576 * 2;
            if (namefile.Length > mb)
            {
                Console.WriteLine("Размер файла" + file + "превышает 2Мб.  Измените файл и запустите программу заново");
                return true;
            }
            else return false;

        }

        static void Main (string[] args)
		{
            try
            {
            link: Console.WriteLine("Введите номер задания для исполнения: \n 1  тестовое \n 2  весна... \n 3  стихотворение(глаголы) \n 4  стихотворение (телефон, Чуковский) \n 5  проза (Что такое жизнь?)");
                Console.Write("Вы выбрали ");
                string x = Console.ReadLine();				// выбор варианта текст-словарь с клавиатуры
                if ((x == "1") || (x == "2") || (x == "3") || (x == "4") || (x == "5"))
                {
                    string file = "vocabulary" + x + ".txt";				//открытие файла словаря	
                    if (FileBig(file) == true) goto link;             //проверка файла на превышение 2Мб
                    string[] readVoc = File.ReadAllLines(file);
                    file = "text" + x + ".txt";
                    if (FileBig(file) == true) goto link;
                    StreamReader text = new StreamReader(file); //    создаем «потоковый читатель» и связываем его с файловым потоком 			 
                    FileStream file1 = new FileStream("result.html", FileMode.Create);    //создаем файл для записи результата
                    StreamWriter writer = new StreamWriter(file1, Encoding.UTF8);//  Encoding.UTF8  or Encoding.Unicode			
                    int kolstr = 0;
                    const int N = 20; // константа количества строк в выходном файле
                    int nn = N;
                    for (int i = 1; text.EndOfStream == false; i++)
                    {
                        string f = text.ReadLine();		 //исходный текст из файла построчно 
                        f = f.Trim();					// удаление знака пробела в начале и в конце строки
                        if (String.IsNullOrEmpty(f) == true) // проверка на пустые строки из файла с текстом
                        {
                            f = text.ReadLine();
                            f = f.Trim();
                            i++;
                        }
                        kolstr++;
                        x = newSt((CreateCol(f)), readVoc);
                        char[] point =  {'.','!','?'};
                        if (kolstr >= nn)
                        {
                            foreach (char t in point)		//перебор  слов из словаря для проверки соответствия
                            {
                                    if (x.Contains(t))
                                    {
                                        if (x[x.Length - 1] == t)
                                           {
                                              writer.WriteLine(x);
                                              writer.Close();
                                              file1 = new FileStream("result" + (kolstr + 1) + ".html", FileMode.Create);    //создаем файл для записи результата
                                              writer = new StreamWriter(file1, Encoding.UTF8);//  Encoding.UTF8  or Encoding.Unicode
                                              nn = nn + N;
                                              break;
                                           }
                                        else
                                        { 
                                            string[] xx2 = x.Split(t);
                                            writer.WriteLine(xx2[0]);
                                            writer.WriteLine(t);
                                            writer.Close();
                                            file1 = new FileStream("result" + (kolstr + 1) + ".html", FileMode.Create);    //создаем файл для записи результата
                                            writer = new StreamWriter(file1, Encoding.UTF8);//  Encoding.UTF8  or Encoding.Unicode
                                            nn = nn + N;
                                            writer.WriteLine(xx2[1]);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (t == '?')
                                        {
                                            writer.WriteLine(x);
                                            writer.WriteLine("<br/>");
                                        }
                                    }
                              }
                        }
                        else
                        {
                            writer.WriteLine(x);
                            writer.WriteLine("<br/>");
                        }
                    }
                    writer.Close();
                    text.Close(); //закрываем поток	
                }
                else
                {
                    Console.WriteLine("Введите правильное значение (1,2,3,4 или 5)");
                    goto link;
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Уберите из входных файлов пустые строки в конце.");
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл отсутствует. Проверьте наличие файлов в папке Debug: \n vocabulary.txt - файл словарь, \n text.txt - файл с текстом.");
            }
            catch (IOException)
            {
                Console.WriteLine("Проверьте, данный файл может быть занят другой программой.");
            }
            Console.ReadLine();
			
			
		}
	}
}
