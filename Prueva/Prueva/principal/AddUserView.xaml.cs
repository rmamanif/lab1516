using Newtonsoft.Json;
using Prueva.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prueva.principal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserView : ContentPage
    {
        public AddUserView()
        {
            InitializeComponent();
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    await DisplayAlert("Advertencia", "El campo es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    await DisplayAlert("Advertencia", "El campo es obligatorio", "OK");
                }
                else if (String.IsNullOrWhiteSpace(txtTipo.Text))
                {
                    await DisplayAlert("Advertencia", "El campo es obligatorio", "OK");
                }
                else
                {
                    var user = new User();
                    user.id = 0;
                    user.name = txtUsuario.Text;
                    user.rating = int.Parse(txtContrasena.Text);
                    user.category = txtTipo.Text;
                    user.release_date = txtFecha.Date.ToString("yyyy-MM-dd");
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://api1516.herokuapp.com/series/");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Accpet", "application/json");
                    var payload = JsonConvert.SerializeObject(user);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    request.Content = c;
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DisplayAlert("Notificación", "Película creada con éxito" + txtUsuario.Text, "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Notificación","Error al insertar la película", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notificación",Convert.ToString(ex), "OK");
                await Navigation.PopToRootAsync();
            }
        }
    }
}