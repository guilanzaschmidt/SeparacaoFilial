using SeparacaoFilial.DTO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparacaoFilial.Pages
{
    public class BasePage : ContentPage
    {
        public string LabelUsuario { get; set; }
        public string LabelProduto { get; set; }
        public string LabelEndereco { get; set; }
        public string LabelResultado { get; set; }

        private protected MenuPage _menuPage;
        private protected UsuarioDTO _usuario;
        private protected GerarCarga _gerarcarga;
        private protected string numeroOP;
        private protected string quantidade;

        private protected async void FocarCampoInicial(Entry campoInicial)
        {
            await Task.Delay(100);
            campoInicial.Focus();
        }

        private protected bool ClicarBotaoVoltar(string textoVerificacao)
        {
            if (!string.IsNullOrEmpty(textoVerificacao))

                Device.BeginInvokeOnMainThread(async () =>
                {
                    var resposta = await DisplayAlert("Atenção!", "A OP ainda não foi enviada, deseja realmente voltar para o menu principal?", "Sim", "Não");

                    if (resposta)
                        Application.Current.MainPage = _menuPage;
                    //Application.Current.MainPage = _gerarcarga;
                });
            else
                Application.Current.MainPage = _menuPage;
            //Application.Current.MainPage = _gerarcarga;

            return true;
        }

        private protected string ExtrairNumeroOPCodigoBarrasOP(string codigoBarrasOP)
        {
            try
            {
                numeroOP = codigoBarrasOP.Trim().Substring(6, 6);
            }
            catch
            {
                numeroOP = string.Empty;
                DisplayAlert("Erro!", "Falha ao extrair o número da OP do código de barras.", "OK");
            }

            return numeroOP;
        }

        private protected void PreencherUsuario(string usuario)
        {
            LabelUsuario = usuario;
            OnPropertyChanged(nameof(LabelUsuario));
        }

        private protected void MostrarMensagemResultado(string mensagemResultado)
        {
            LabelResultado = mensagemResultado;
            OnPropertyChanged(nameof(LabelResultado));
        }

        private protected void MostrarEndereco(string endereco)
        {
            LabelEndereco = endereco;
            OnPropertyChanged(nameof(LabelEndereco));
        }

        private protected void PreencherProduto(string produto)
        {
            LabelProduto = produto;
            OnPropertyChanged(nameof(LabelProduto));
        }

        private protected void LimparCampos(Entry txtCodigoBarrasOp, Entry txtQuantidadeProduto)
        {
            txtCodigoBarrasOp.Text = string.Empty;
            txtQuantidadeProduto.Text = string.Empty;

            txtCodigoBarrasOp.Focus();
        }
    }
}