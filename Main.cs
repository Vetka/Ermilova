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
		static void Main (string[] args)
		{
            try
            {
            link: Console.WriteLine("Введите номер задания для исполнения: \n 1  тестовое \n 2  весна... \n 3  стихотворение(глаголы) \n 4  стихотворение (телефон, Чуковский)");
                Console.Write("Вы выбрали ");
                string x = Console.ReadLine();				// выбор варианта текст-словарь
                if ((x == "1") || (x == "2") || (x == "3") || (x == "4"))
                {
                    string file = "vocabulary" + x + ".txt";				//открытие файла словаря	

                    FileInfo text1 = new FileInfo("vocabulary" + x + ".txt");   //проверка превышает ли файл 2Мб
                    long mb = 1048576 * 2;
                    if (text1.Length > mb)
                    {
                        Console.WriteLine("Размер файла  vocabulary" + x + ".txt превышает 2Мб.  Измените файл и запустите программу заново");
                        goto fun;
                    }

                    FileInfo text2 = new FileInfo("text" + x + ".txt");
                    if (text2.Length > mb)
                    {
                        Console.WriteLine("Размер файла  text" + x + ".txt превышает 2Мб. Измените файл и запустите программу заново");
                        goto fun;
                    }
 
                    const int N=20; // константа количества строк в выходном файле
                    int nn=N;
                    
                    string[] readVoc = File.ReadAllLines(file);
                    file = "text" + x + ".txt";                  
                    StreamReader text = new StreamReader(file); //    создаем «потоковый читатель» и связываем его с файловым потоком 			 
                    FileStream file1 = new FileStream("result.html", FileMode.Create);    //создаем файл для записи результата
                    StreamWriter writer = new StreamWriter(file1, Encoding.UTF8);//  Encoding.UTF8  or Encoding.Unicode			
                    int kolstr = 0;
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
                       
                        List<string> kol = new List<string>(f.Split(' '));  //разбивка строки на слова и запись в коллекцию				
                        string znak = ".,;!?-:\"";
                        for (int tt = 0; tt < kol.Count; tt++)  // перебор слов в строке
                        {
                            string p = kol[tt];			// p присваивается значение слова из массива
                            char[] buk = p.ToCharArray();
                            int k = buk.Length;
                            k--;
                            if (znak.Contains(buk[k]))		// проверка является ли последний символ в слове знаком препинания							
                            {
                                string zn = Convert.ToString(buk[k]);
                                p = p.Remove(k);				// отрезаем знак от слова
                                kol[tt] = p;					//запись нового слова
                                tt++;
                                kol.Insert(tt, zn);			//запись знака в коллекци, дабы его не потерять																								
                            }
                          
                        }
                        for (int tt = 0; tt < kol.Count; tt++)		//перебор значения коллекции
                        {
                            foreach (string vk in readVoc)		//перебор  слов из словаря для проверки соответствия
                            {
                                string t = vk.Trim();				// удаление знака пробела в начале и в конце строки
                                if (String.Equals(t, kol[tt], StringComparison.CurrentCultureIgnoreCase))	//в случае совпадения со словом из словаря (без учета реестра), делает его жирным и курсивом
                                    kol[tt] = "<b><i>" + kol[tt] + "</b></i>";
                            }
                           
                            if (znak.Contains(kol[tt]))           //запись слов в файл результат 
                                writer.Write(kol[tt]);
                            else
                                writer.Write(" " + kol[tt]);
                            string point = ".;!?:";
                            if ((kolstr >= nn) && (point.Contains(kol[tt])) && (tt == (kol.Count-1)))  // || (point == "!") || (point == "?") || (point == "-") || (point == ","))
                            {
                                writer.Close();
                                file1 = new FileStream("result" + (kolstr+1) + ".html", FileMode.Create);    //создаем файл для записи результата
                                writer = new StreamWriter(file1, Encoding.UTF8);//  Encoding.UTF8  or Encoding.Unicode
                                nn = nn+N;
                               // writer.WriteLine("Продолжение...");
                            }
                        }
                       writer.WriteLine("<br/>");
                       
                    }
                    writer.Close();
                    text.Close(); //закрываем поток	
                    
                    //Console.WriteLine("Результат обработки файлов Вы найдете в файле result.html");
                    //Console.WriteLine("Открыть файл с результатами (y/n)");
                    //x = Console.ReadLine();
                    //if (x == "y")
                    //{
                    //    Process.Start("result.html");
                    //}
                }
                else
                {
                    Console.WriteLine("Введите правильное значение (1,2,3 или 4)");
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
         fun:   Console.ReadLine();
			
			
		}
	}
}
