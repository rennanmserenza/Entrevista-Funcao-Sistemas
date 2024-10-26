using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL.Beneficiarios
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiario
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal long Incluir(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IdCliente", beneficiario.IdCliente),
            };

            DataSet ds = Consultar("FI_SP_IncBeneficiario", parametros);

            long ret = 0;

            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);

            return ret;
        }

        /// <summary>
        /// Consultar um novo beneficiario
        /// </summary>
        /// <param name="Id">Id do beneficiario</param>
        internal Beneficiario Consultar(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            DataSet ds = Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> ben = Converter(ds);

            return ben.FirstOrDefault();
        }

        /// <summary>
        /// Verifica a existência de beneficiario cadastrado para o cliente selecionado.
        /// </summary>
        /// <param name="Id">Id do beneficiario</param>
        internal bool VerificarExistencia(string CPF, int idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", CPF),
                new SqlParameter("IdCliente", idCliente)
            };

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Pesquisa a lista de beneficiarios
        /// </summary>
        /// <param name="iniciarEm"></param>
        /// <param name="quantidade"></param>
        /// <param name="campoOrdenacao"></param>
        /// <param name="crescente"></param>
        /// <param name="qtd"></param>
        /// <returns>Lista de beneficiarios</returns>
        internal List<Beneficiario> Pesquisa(
            int iniciarEm, 
            int quantidade, 
            string campoOrdenacao, 
            bool crescente, 
            out int qtd)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("iniciarEm", iniciarEm),
                new SqlParameter("quantidade", quantidade),
                new SqlParameter("campoOrdenacao", campoOrdenacao),
                new SqlParameter("crescente", crescente)
            };

            DataSet ds = Consultar("FI_SP_PesqBeneficiario", parametros);
            List<Beneficiario> ben = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return ben;
        }

        /// <summary>
        /// Lista todos os beneficiarios
        /// </summary>
        internal List<Beneficiario> Listar()
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", 0)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> ben = Converter(ds);

            return ben;
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal void Alterar(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IdCliente", beneficiario.IdCliente),
                new SqlParameter("ID", beneficiario.Id)
            };

            Executar("FI_SP_AltBeneficiario", parametros);
        }

        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="Id">Id do beneficiario</param>
        internal void Excluir(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            Executar("FI_SP_DelBeneficiario", parametros);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario cli = new Beneficiario
                    {
                        Id = row.Field<long>("Id"),
                        Nome = row.Field<string>("Nome"),
                        CPF = row.Field<string>("CPF")
                    };

                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
