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
    public partial class DetaUserView : ContentPage
    {
        public int id_user;
        public DetaUserView(int Id_User)
        {
            InitializeComponent();
            id_user = Id_User;
            EdittextFalse();
            btnGuardar.Clicked += BtnGuardar_Clicked;
            btnEliminar.Clicked += BtnEliminar_Clicked;
        }

        private void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Notificación", "¿Realmente Desea eliminarlo?", "Si", "No");
                if (result)
                {
                    try
                    {
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri("https://api1516.herokuapp.com/series/" + id_user+"/");
                        request.Method = HttpMethod.Delete;
                        request.Headers.Add("Accpet", "application/json");
                        var client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(request);
                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            await DisplayAlert("Notificación", "Eliminado", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "Error al conectar", "OK");
                            await Navigation.PopToRootAsync();
                        }
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    EdittextFalse();
                }
            });
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Notificación", "¿Realmente Desea modifícarlo?", "Si", "No");
                if (result)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(txtUsuario.Text))
                        {
                            await DisplayAlert("Advertencia", "El campo Usuario es obligatorio", "OK");
                        }
                        else if (String.IsNullOrWhiteSpace(txtContrasena.Text))
                        {
                            await DisplayAlert("Advertencia", "El campo Contraseña es obligatorio", "OK");
                        }
                        else
                        {
                            var user = new User();
                            user.id = id_user;
                            user.name = txtUsuario.Text;
                            user.rating = int.Parse(txtContrasena.Text);
                            user.category = txtTipo.Text;
                            user.release_date = txtFecha.Date.ToString("yyyy-MM-dd");
                            
                            var request = new HttpRequestMessage();
                            request.RequestUri = new Uri("https://api1516.herokuapp.com/series/" + id_user+"/");
                            request.Method = HttpMethod.Put;
                            request.Headers.Add("Accpet", "application/json");
                            var payload = JsonConvert.SerializeObject(user);
                            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                            request.Content = c;
                            var client = new HttpClient();
                            HttpResponseMessage response = await client.SendAsync(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                await DisplayAlert("Notificación", "Se ha modificado con éxito :" + txtUsuario.Text, "OK");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Notificación", "xd", "OK");
                                await Navigation.PopToRootAsync();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    OnAppearing();
                    EdittextFalse();
                }
            });
        }

        private void EdittextFalse()
        {
            txtUsuario.IsEnabled = false;
            txtContrasena.IsEnabled = false;
            txtTipo.IsEnabled = false;
            btnGuardar.IsVisible = false;
        }

        private void MenuItem1_Clicked(object sender, EventArgs e)
        {
            txtUsuario.IsEnabled = true;
            txtContrasena.IsEnabled = true;
            txtTipo.IsEnabled = true;
            btnGuardar.IsVisible = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://api1516.herokuapp.com/series/" + id_user+"/");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accpet", "application/json");
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<User>(content);
                        if (resultado != null)
                        {
                            txtUsuario.Text = resultado.name;
                            txtContrasena.Text = Convert.ToString(resultado.rating);
                            txtTipo.Text = resultado.category;
                            txtFecha.Date = DateTime.Parse(resultado.release_date);
                        }
                        else
                        {
                            await DisplayAlert("Notificación", "Error al conectar", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Notificación", "Error al conectar", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Notificación", Convert.ToString(ex), "OK");
                    await Navigation.PopToRootAsync();
                }
            });
        }
    }
}