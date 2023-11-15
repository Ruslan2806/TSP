using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace TSP
{
    internal class TSP // Задача коммивояжёра
    {
        static int[,] CreateAdjacencyMatrix() // функция для создания матрицы смежности
        {
            WriteLine("Заполните таблицу смежности.\nВведите количество городов: ");
            int countCities = int.Parse(ReadLine());
            int[,] adjMatr = new int[countCities, countCities];
            for (int i = 0; i < countCities; i++)
            {
                for (int j = 0; j < countCities; j++)
                {
                    if (i == j)
                    {
                        adjMatr[i, j] = int.MaxValue;
                    }
                    else
                    {
                        WriteLine($"Введите расстояние от города {i + 1} до города {j + 1}: ");
                        int distance = int.Parse(ReadLine());
                        if (distance < 0)
                        {
                            adjMatr[i, j] = 0;
                        }
                        else
                        {
                            adjMatr[i, j] = distance;
                        }
                    }
                }
            }
            return adjMatr;
        }

        static int[,] ReadMatrixFromFile(string filePath) // Функция для считывания матрицы смежности из файла
        {
            string[] lines = File.ReadAllLines(filePath);
            int rowCount = lines.Length;
            int columnCount = lines[0].Split().Length;
            int[,] matrix = new int[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] values = lines[i].Split();
                for (int j = 0; j < columnCount; j++)
                {
                    if (values[j] == "М" || values[j] == "M")
                    {
                        matrix[i, j] = int.MaxValue;
                    }
                    else
                    {
                        matrix[i, j] = int.Parse(values[j]);
                    }
                }
            }

            return matrix;
        }

        static int[,] PutAdjMatr() //Функция для выбора способа инициализации матрицы смежности
        {
            while (true)
            {
                WriteLine("Выберите, каким спобом Вы хотите задать матрицу смежности?" +
               "\n -> 1 (задать с помощью .txt файла)" +
               "\n -> 2 (ввести вручную)");
                int a = int.Parse(ReadLine());
                if (a == 2) return CreateAdjacencyMatrix();
                else if (a == 1)
                {
                    WriteLine("\nПример содержимого файла:" +
                              "\nM 1 2 5 9\r\n4 M 5 3 3\r\n1 1 M 1 2\r\n4 3 6 M 5\r\n5 6 5 8 M\n");
                    WriteLine("Введите название файла (например: \"matr.txt\")");
                    string path = ReadLine();
                    return ReadMatrixFromFile(path);
                }
                else WriteLine("Ошибка!");
            }
        }

        static void FindMinCostPath(int[,] adjMatr, int currCity, int level, int cost, int[] currPath,  //функция для нахождения минимального пути и его расстояния
            ref int minCost, ref int[] minPath, bool[] visited)
        {
            int size = adjMatr.GetLength(0);

            if (level == size - 1)
            {
                int lastCity = 0;
                for (int i = 0; i < size; i++)
                {
                    if (!visited[i])
                    {
                        lastCity = i;
                        break;
                    }
                }

                cost += adjMatr[currCity, lastCity];
                cost += adjMatr[lastCity, 0];

                if (cost < minCost)
                {
                    minCost = cost;
                    Array.Copy(currPath, minPath, size);
                    minPath[size - 1] = lastCity;
                }

                return;
            }

            for (int i = 0; i < size; i++)
            {
                if (!visited[i] && adjMatr[currCity, i] != int.MaxValue)
                {
                    visited[i] = true;
                    currPath[level] = i;
                    FindMinCostPath(adjMatr, i, level + 1, cost + adjMatr[currCity, i], currPath, ref minCost, ref minPath, visited);
                    visited[i] = false;
                    currPath[level] = -1;
                }
            }
        }

        static void Main(string[] args)
        {

            WriteLine("\r\n  _                       _ _                  \r\n | |                     | (_)                 \r\n | |_ _ _" +
                "_ __ ___   _____| |_ _ __   __ _      \r\n | __| '__/ _` \\ \\ / / _ \\ | | '_ \\ / _` |     \r\n | |_| | | (_| |\\ V /  __/ | | | | | (_| |  " +
                "   \r\n  \\__|_|  \\__,_| \\_/ \\___|_|_|_| |_|\\__, |     \r\n           | |                       __/ |     \r\n  ___  __ _| | ___  ___ _ __ " +
                "___   _|___/ __   \r\n / __|/ _` | |/ _ \\/ __| '_ ` _ \\ / _` | '_ \\  \r\n \\__ \\ (_| | |  __/\\__ \\ | | | | | (_| | | | | \r\n |___/\\__,_" +
                "|_|\\___||___/_| |_| |_|\\__,_|_| |_| \r\n                 | |   | |                     \r\n  _ __  _ __ ___ | |__ | | ___ _ __ ___      " +
                "  \r\n | '_ \\| '__/ _ \\| '_ \\| |/ _ \\ '_ ` _ \\       \r\n | |_) | | | (_) | |_) | |  __/ | | | | |      \r\n | .__/|_|  \\___/|_._" +
                "_/|_|\\___|_| |_| |_|      \r\n | |                                           \r\n |_|                                           \r\n");

            while (true)
            {
                int[,] adjMatr = PutAdjMatr();
                int size = adjMatr.GetLength(0);
                int[] currPath = new int[size];
                int[] minPath = new int[size];
                bool[] visited = new bool[size];
                int minCost = int.MaxValue;
                visited[0] = true;
                currPath[0] = 0;
                FindMinCostPath(adjMatr, 0, 1, 0, currPath, ref minCost, ref minPath, visited);
                Write("\nОПТИМАЛЬНЫЙ ПУТЬ: ");
                for (int i = 0; i < size; i++) if (i + 1 != size) Write($"({minPath[i] + 1}) -> ");
                    else Write($"({minPath[i] + 1}) -> (1)\n");
                WriteLine("МИНИМАЛЬНАЯ ДЛИНА ПУТИ: " + minCost + "\n");
                WriteLine("1 - выход");
                int a = int.Parse(ReadLine());
                if (a == 1) break;
            }
        }
    }
}
