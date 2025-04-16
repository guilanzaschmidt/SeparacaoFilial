namespace SeparacaoFilial.Utils
{
    internal sealed class Constantes
    {
        internal const string DNS_SERVIDOR_TESTES = "srvv64";
        internal const string DNS_SERVIDOR_OFICIAL = "srvv44";
        internal const string DNS_SERVIDOR_XT = "srvv17";

        internal const string IP_SERVIDOR_TESTES = "10.1.0.62"; //10.1.0.62 Teste 64 / 10.1.0.209 Servidor Teste XT
        internal const string IP_SERVIDOR_OFICIAL = "10.1.0.83";
        //internal const string IP_SERVIDOR_OFICIAL = "10.3.2.30"; comentário Felipe teste repositório

        internal const int REGRA_FUNCAO_AUX_TED_CONTROL = 118;
        internal const int REGRA_TED_CONTROL_OPS = 799;
        internal const int REGRA_VERIFICA_SE_MERCADO = 183;
        internal const int REGRA_GERAR_CARGA = 183;

        internal const int QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_UNICO = 50; //Era 48
        internal const int QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_COMPLETO = 20;
        internal const int QTDE_DIGITOS_CODIGO_DE_BARRAS_ACESSORIOS = 11;

        internal const char FORCAR = 'S';
        internal const char NAO_FORCAR = 'N';
        internal const char TIPO = 'S';

        internal const string DEP_MERCADO = "200";
    }
}