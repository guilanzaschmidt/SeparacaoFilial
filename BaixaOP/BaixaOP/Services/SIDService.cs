using SeparacaoFilial.Utils;
using System;
using System.IO;
using System.Net;

namespace SeparacaoFilial.Services
{
    internal class SIDService
    {
        private static string urlAutenticacaoUsuario;

        private static HttpWebRequest httpWebRequest;
        private static Stream stream;
        private static StreamReader streamReader;

        internal static string ValidarCredenciaisUsuario(string ipServidor, string usuario, string senha)
        {
            urlAutenticacaoUsuario = "http://" + ipServidor + ":8080/sapiensweb/conector?SIS=CO&LOGIN=SID&ACAO=EXESENHA&NOMUSU=" + usuario + "&SENUSU=" + senha;

            return ExecutarRequest(urlAutenticacaoUsuario);
        }

        internal static string ExecutarRegraSeparacaoFilial(string codBar, string usuario, string senha)
        {
            var urlRegraSeparacaoFilial = urlAutenticacaoUsuario + "&PROXACAO=SID.Srv.Regra&NumReg=" + Constantes.REGRA_TED_CONTROL_OPS +
                "&aCodBar=" + codBar + "&VCNomUsu=" + usuario + "&VCSenha=" + senha + "&aForcaOP=" + Constantes.NAO_FORCAR;

            return ExecutarRequest(urlRegraSeparacaoFilial);
        }

        internal static string ExecutarRegraGerarCarga()
        {
            var urlRegraSeparacaoFilial = urlAutenticacaoUsuario + "&PROXACAO=SID.Srv.Regra&NumReg=" + Constantes.REGRA_GERAR_CARGA;

            return ExecutarRequest(urlRegraSeparacaoFilial);
        }

        internal static string ExecutarRegraSeparacaoFilialAlmoxarifado(string codBar, string usuario, string senha)
        {
            var urlRegraSeparacaoFilialAlmoxarifado = urlAutenticacaoUsuario + "&PROXACAO=SID.Srv.Regra&NumReg=" + Constantes.REGRA_FUNCAO_AUX_TED_CONTROL +
                "&aCodBar=" + codBar + "&VCNomUsu=" + usuario + "&VCSenha=" + senha;

            return ExecutarRequest(urlRegraSeparacaoFilialAlmoxarifado);
        }

        internal static string ExecutarRegraVerificaOPMercado(string codBar)
        {
            var urlRegraVerificaOPMercado= urlAutenticacaoUsuario + "&PROXACAO=SID.Srv.Regra&NumReg=" + Constantes.REGRA_VERIFICA_SE_MERCADO +
                "&aCodBar=" + codBar;

            return ExecutarRequest(urlRegraVerificaOPMercado);
        }

        private static string ExecutarRequest(string url)
        {
            string textoRetorno;

            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = 0;

                stream = httpWebRequest.GetRequestStream();
                stream.Close();

                streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), System.Text.Encoding.UTF7);

                textoRetorno = streamReader.ReadToEnd().Trim();

                streamReader.Close();
                streamReader.Dispose();

            } catch (Exception ex)
            {
                textoRetorno = ex.Message;
            }

            return textoRetorno;
        }
    }
}