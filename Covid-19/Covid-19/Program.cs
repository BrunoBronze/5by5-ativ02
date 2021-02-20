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
                                  "3 - Nao sei\n" +
                                  "4 - Nao sei\n" +
                                  "5 - Nao sei\n" +
                                  "6 - Encerrar programa"); //Imprime o proximo e retira da fila.
                Console.Write("\n>>>");
                op = Console.ReadLine();

                switch (op)
                {
                    case "1":
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
                        Console.WriteLine("Chamando próximo paciente para exame...");

                        if (!filaPrioritaria.Vazia() && contador < 2)
                        {
                            paciente = filaPrioritaria.Head;
                            filaPrioritaria.Pop();
                            contador++;

                            Exames(paciente);
                        }
                        else if (!fila.Vazia())
                        {
                            paciente = fila.Head;
                            fila.Pop();
                            contador = 0;

                            Exames(paciente);

                        }
                        else
                        {
                            Console.WriteLine("Não há ninguem na fila!");
                            contador = 0;
                        }

                        break;

                    case "6":
                        Console.WriteLine("FINALIZANDO");
                        break;

                    default:
                        break;
                }
            } while (op != "6");
        }

        static Paciente Leitura(string cpf)
        {
            Console.WriteLine($"\nDigite os dados do CPF:  {cpf}");

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
            ArquivoCSV arquivo = new ArquivoCSV();
            string[] propriedades = arquivo.Leitura();

            Paciente paciente = new Paciente
            {
                Nome = propriedades[0],
                CPF = "43831426805",
                DataNascimento = DateTime.ParseExact("24/03/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Telefone = "(16) 997252079",
                Proximo = null
            };
            Console.WriteLine();
            return paciente;
        }
        static void Exames(Paciente paciente)
        {
            bool resultado;

            Console.WriteLine(paciente);
            Console.WriteLine();

            resultado = paciente.VerificaStatus();

            if (paciente.Covid == false)
            {
                Console.WriteLine("Arquivando paciente...");
                //pacienteE.Arquivar();
            }
            else
            {
                paciente.Importancia();
            }
        }
    }
}