using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.WebApi;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace geovisual.Controllers
{
    [RoutePrefix("xyz")]
    public class XyzController : ApiController
    {
        [HttpGet]
        [Route("{z}/{x}/{y}")]
        public IHttpActionResult GetMap(int z, int x, int y)
        {
            
            ShapefileLayer layer = new ShapefileLayer(HttpContext.Current.Server.MapPath("~/App_Data/world-bound-900913.shp"));
            layer.Styles.Add(new FillStyle(GeoColor.FromHtml("#AAFFDF3E"), GeoColors.White));

            ShapefileLayer layer2 = new ShapefileLayer(HttpContext.Current.Server.MapPath("~/App_Data/user_timelines_since_t1_w_coordinates.shp"));
            layer2.Styles.Add(new FillStyle(GeoColor.FromHtml("#AAFFDF3E"), GeoColors.Red));

            MapModel mapModel = new MapModel(GeoUnit.Meter);
            mapModel.Layers.Add(layer);
            mapModel.Layers.Add(layer2);

            return new XyzTileResult(mapModel, x, y, z);
        }

        [HttpGet]
        [Route("identify")]
        public IHttpActionResult Identify(double x, double y, int z)
        {
            ShapefileLayer layer = new ShapefileLayer(HttpContext.Current.Server.MapPath("~/App_Data/world-bound-900913.shp"));
            Feature feature = layer.Identify(new GeoCoordinate(x, y), new ScaleLevels()[z].Scale, GeoUnit.Meter).FirstOrDefault();
            if (feature != null)
            {
                return Json(feature);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
