using SeparacaoFilial.DTO;
using SeparacaoFilial.Services;
using SeparacaoFilial.Utils;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparacaoFilial.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BasePage
    {
        public LoginPage()
        {
            InitializeComponent();

            FocarCampoInicial(txtUsuario);
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var resposta = await DisplayAlert("Atenção!", "Deseja realmente sair do aplicativo?", "Sim", "Não");

                if (resposta)
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            });

            return true;
        }

        private void TxtUsuarioOnEnterKeyPressed(object sender, EventArgs e) => txtSenha.Focus();

        private void TxtSenhaOnEnterKeyPressed(object sender, EventArgs e) => pckFilial.Focus();
        private void pckFilial_SelectedIndexChanged(object sender, EventArgs e) => BtnLoginClicked(sender, e);

        private void BtnLoginClicked(object sender, EventArgs e)
        {
            if (ValidarCampos())
                LogIn();
        }

        private bool ValidarCampos()
        {
            bool camposValidos = false;

            if (string.IsNullOrEmpty(txtUsuario.Text) &&
            string.IsNullOrEmpty(txtSenha.Text) ||
            string.IsNullOrEmpty(txtUsuario.Text.Trim()) &&
            string.IsNullOrEmpty(txtSenha.Text.Trim()))
            {
                DisplayAlert("Atenção!", "Digite as credenciais.", "OK");
                LimparCampos();
            }
            else if (string.IsNullOrEmpty(txtUsuario.Text) ||
            string.IsNullOrEmpty(txtUsuario.Text.Trim()))
            {
                DisplayAlert("Atenção!", "Digite o usuário.", "OK");

                txtUsuario.Text = string.Empty;
                txtUsuario.Focus();
            }
            else if (string.IsNullOrEmpty(txtSenha.Text) ||
            string.IsNullOrEmpty(txtSenha.Text.Trim()))
            {
                DisplayAlert("Atenção!", "Digite a senha.", "OK");

                txtSenha.Text = string.Empty;
                txtSenha.Focus();
            }
            else
                camposValidos = true;

            return camposValidos;
        }

        private async void LogIn()
        {
            await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

            var usuario = new UsuarioDTO
            {
                Usuario = txtUsuario.Text.Trim(),
                Senha = txtSenha.Text.Trim(),
                Filial = pckFilial.Items[pckFilial.SelectedIndex].EndsWith("1") ? "1" : "2"
            };

            var resultado = SIDService.ValidarCredenciaisUsuario(Constantes.DNS_SERVIDOR_OFICIAL, usuario.Usuario, usuario.Senha);
            //var resultado = "tt";
            var posicao = resultado.ToUpper().IndexOf("ERR");
            //var posicao = -1;
            await PopupNavigation.Instance.PopAsync();

            if (posicao != -1)
            {
                await DisplayAlert("Erro!", "Não foi possível conectar ao servidor " + Constantes.DNS_SERVIDOR_OFICIAL + ". " +
                    resultado, "OK");

                LimparCampos();
            }
            else
                Application.Current.MainPage = new MenuPage(usuario);
            //Application.Current.MainPage = new GerarCarga(usuario);

        }

        private void LimparCampos()
        {
            txtUsuario.Text = string.Empty;
            txtSenha.Text = string.Empty;

            txtUsuario.Focus();
        }
    }
}