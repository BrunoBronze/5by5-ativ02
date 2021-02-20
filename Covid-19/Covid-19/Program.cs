using System;
using System.Globalization;

namespace Covid_19
{
    class Program
    {
        static void Main(string[] args)
        {
            Paciente paciente = new Paciente();
            FilaPacientes fila = new FilaPacientes();
            FilaPacientes filaPrioritaria = new FilaPacientes();
            FilaPacientes filaInternacao = new FilaPacientes();
            int contador = 0;

            string op;
            do
            {
                Console.WriteLine("###### COVID 19 ######\n" +
                                  "1 - Cadastre um paciente\n" +
                                  "2 - Proximo da fila\n" +
                                  "3 - Encerrar programa"); //Imprime o proximo e retira da fila.
                Console.Write("\n>>>");
                op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("\nInforme o CPF: ");
                        string cpf = Console.ReadLine();

                        if (CpfCadastrado())
                        {
                            paciente = LeituraArquivo(cpf);
                        }
                        else
                        {
                            paciente = Leitura(cpf);
                        }

                        if (paciente.Idade() >= 60)
                        {
                            filaPrioritaria.Push(paciente);
                        }
                        else
                        {
                            fila.Push(paciente);
                        }
                        break;

                    case "2":
                        Console.Clear();
                        

                        if (!filaPrioritaria.Vazia() && contador < 2)
                        {
                            Console.WriteLine("Chamando próximo paciente para exame...");
                            paciente = filaPrioritaria.Head;
                            filaPrioritaria.Pop();
                            contador++;

                            Imprimir(paciente);
                            Infectado(paciente);
                        }
                        else if (!fila.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para exame...");
                            paciente = fila.Head;
                            fila.Pop();
                            contador = 0;
                            
                            Imprimir(paciente);
                            Infectado(paciente);
                        }
                        else
                        {
                            Console.WriteLine("Não há ninguem na fila!");
                            contador = 0;
                        }

                        break;

                    case "3":
                        Console.WriteLine(">>> FINALIZANDO <<<");
                        break;

                    default:
                        break;
                }
            } while (op != "3");
        }

        static Paciente Leitura(string cpf)
        {
            //Console.WriteLine($"\nCPF digitado {cpf}");

            Console.Write("Nome :");
            string nome = Console.ReadLine();


            Console.Write("Data de nascimento(dd/mm/aaaa): ");
            DateTime dataNascimento = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Paciente paciente = new Paciente
            {
                Nome = nome,
                CPF = cpf,
                DataNascimento = dataNascimento,
                Telefone = telefone,
                Proximo = null
            };

            Console.WriteLine();
            return paciente;
        }
        static bool CpfCadastrado()
        {
            return false;
        }

        static Paciente LeituraArquivo(string cpf)
        {
            Console.Clear();
            ArquivoCSV arquivo = new ArquivoCSV();
            string[] propriedades = arquivo.Leitura();

            Paciente paciente = new Paciente
            {
                Nome = "Bruno Bronze",
                CPF = "43831426805",
                DataNascimento = DateTime.ParseExact("24/03/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Telefone = "(16) 997252079",
                Proximo = null
            };
            Console.WriteLine();
            return paciente;
        }
        static void Imprimir(Paciente paciente)
        {
            Console.Clear();
            Console.WriteLine(paciente);
            Console.WriteLine();
        }

        static void Infectado(Paciente paciente)
        {
            paciente.VerificaStatus();

            if (paciente.Covid)
            {

                paciente.Importancia();

                Console.Write("Mandará para internação (s/n)?");
                string status = Console.ReadLine();

                if (status.ToLower() == "s")
                {
                    Console.WriteLine("\nInternando\n");
                }
                else
                {
                    Console.WriteLine("\nArquivando paciente...\n");
                    //pacienteE.Arquivar();
                }
            }
            else
            {
                Console.WriteLine("\nArquivando paciente...\n");
                //pacienteE.Arquivar();
            }
        }
    }
}