using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace _2doParcialDislaEscanio
{
    public class GenericRepository<TContext> where TContext : class
    {
        public NorthwindEntities Model = new NorthwindEntities();

        public List<T> Listado<T>() where T : class
        {
            return Model.Set<T>().ToList();
        }
        public bool Agregar<T>(T item) where T : class
        {
            try
            {
                Model.Set<T>().Add(item);
                Model.SaveChanges();
                Console.WriteLine("\n ***************** REGISTRO REALIZADO CORRECTAMENTE *****************");
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var entityValidationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return false;
            }
        }
        public bool Actualizar<T>(T item) where T : class
        {
            try
            {
                Model.Entry(item).State = EntityState.Modified;
                Guardar();
                Console.WriteLine("\n ***************** ACTUALIZACIÓN REALIZADA CORRECTAMENTE *****************");
                return true;
            }
            catch (Exception e)
            {
                Console.Write("\n HA OCURRIDO UN ERROR ACTUALIZANDO DATOS: " + e.Message.ToString());
                return false;
            }
        }
        public bool Eliminar<T>(T item) where T : class
        {
            try
            {
                Model.Entry(item).State = EntityState.Deleted;
                Guardar();
                if (item == null)
                {
                    Console.WriteLine("\n ***************** SE HA ELIMINADO EL REGISTRO CORRECTAMENTE *****************");
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var entityValidationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return false;
            }
        }
        public void Guardar()
        {
            try
            {
                Model.SaveChanges();
            }
            catch (Exception)
            {
                Console.Write("\n NO ES POSIBLE ELIMINAR REGISTROS DE ESTA ENTIDAD PORQUE TIENE OTRAS ENTIDADES RELACIONADAS.");
                Console.Write("\n");
            }
        }
    }
}
