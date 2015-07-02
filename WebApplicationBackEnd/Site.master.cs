using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.DynamicData;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace WebApplicationBackEnd
{
    public partial class Site : System.Web.UI.MasterPage
    {

        protected void Button1_Click(object sender, System.EventArgs e)
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            Response.Redirect("~/Default.aspx");
        }
    }
}
