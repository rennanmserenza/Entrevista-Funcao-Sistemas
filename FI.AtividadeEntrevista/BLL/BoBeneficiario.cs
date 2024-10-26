using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Extensions;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(Beneficiario beneficiario)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(Beneficiario beneficiario)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public Beneficiario Consultar(long id)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        public void Excluir(long id)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<Beneficiario> Listar()
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.Listar();
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// Verifica se o CPF recebido é válido
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns>verdadeiro para um CPF válido e falso caso o contrário seja verdadeiro</returns>
        public bool CPFValido(string CPF)
        {
            return StringExtension.ValidarCPF(CPF);
        }

        /// <summary>
        /// Verifica Existencia de um CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, int idCliente)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.VerificarExistencia(CPF, idCliente);
        }
    }
}
