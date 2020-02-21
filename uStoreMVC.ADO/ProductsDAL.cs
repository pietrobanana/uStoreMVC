using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uStoreMVC.Domain;

namespace uStoreMVC.ADO
{
    public class ProductsDAL
    {
        public string GetProductNames()
        {
            string productNames = "<ul>";
            //Create an SQLConnection objects using the following info.
            // Data Source = server name (.\sqlexpress)
            // Initial Catalog = database name (cStore)
            // Integrated Security = credentials (true)
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";
                // Open connection to database
                conn.Open();
                //Create a SqlCommand object (query)
                SqlCommand cmdGetProduct = new SqlCommand("Select * from Products", conn);

                //Execute the command
                //  .ExecuteReader() - Selecting data
                //  .ExeccuteNonQuery() - anything else (create, update, delete)
                SqlDataReader rdrProducts = cmdGetProduct.ExecuteReader();

                //Process the DataReader
                while (rdrProducts.Read())
                {
                    productNames += "<li>" + (string)rdrProducts["ProductName"] + "<li>";
                }

                //Close the connectiong (THIS IS CRITICAL!)
                rdrProducts.Close(); //Manually disposes the rdrAuthors object
                conn.Close();
                productNames += "</ul>";
                return productNames;
            }
            
        } //End using

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            //Create an SQLConnection objects using the following info.
            // Data Source = server name (.\sqlexpress)
            // Initial Catalog = database name (cStore)
            // Integrated Security = credentials (true)
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";


                // Open connection to database
                conn.Open();

                //Create a SqlCommand object (query)
                SqlCommand cmdGetProducts = new SqlCommand("Select * from Products", conn);

                //Execute the command
                //  .ExecuteReader() - Selecting data
                //  .ExeccuteNonQuery() - anything else (create, update, delete)
                SqlDataReader rdrProducts = cmdGetProducts.ExecuteReader();

                //Process the DataReader
                while (rdrProducts.Read())
                {
                    ProductModel pro = new ProductModel()
                    {
                        ProductId = (int)rdrProducts["ProductId"],
                        ProductName = (string)rdrProducts["ProductName"],
                        ProductDescription = (string)rdrProducts["ProductDescription"],
                        Price = (decimal)rdrProducts["Price"]
                    };
                    products.Add(pro);
                }

                //Close the connectiong (THIS IS CRITICAL!)
                rdrProducts.Close();
                conn.Close();
            } //End using
            return products;

        }

        public void CreateProduct(ProductModel product)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";

                conn.Open();

                SqlCommand cmdCreateProduct = new SqlCommand(
                @"Insert into Products
                (ProductName, ProductDescription, Price)
                Values (@ProductName, @ProductDescription, @Price)", conn);

                //Set values for the SQL parameters:
                cmdCreateProduct.Parameters.AddWithValue("ProductName", product.ProductName);
                cmdCreateProduct.Parameters.AddWithValue("ProductDesciption", product.ProductDescription);
                //CITY
                if (product.Price != null)
                {
                    cmdCreateProduct.Parameters.AddWithValue("Price", product.Price);
                }
                else
                {
                    cmdCreateProduct.Parameters.AddWithValue("Price", DBNull.Value);
                }

                cmdCreateProduct.ExecuteNonQuery();
                conn.Close();
            }
        }

        //public ProductModel GetProduct(int id)
        //{
        //    ProductModel product = null;

        //    using (SqlConnection conn = new SqlConnection())
        //    {
        //        conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";
        //        conn.Open();
        //        SqlCommand cmdGetProduct = new SqlCommand(
        //            "Select * from Products where ProductId = @ProductId", conn);
        //        cmdGetProduct.Parameters.AddWithValue("ProductId", id);
        //        SqlDataReader rdrProduct = cmdGetProduct.ExecuteReader();
        //        if (rdrProduct.Read())
        //        {
        //            product = new ProductModel()
        //            {
        //                ProductId = (int)rdrProduct["ProductId"],
        //                FirstName = (string)rdrAuthor["FirstName"],
        //                LastName = (string)rdrAuthor["LastName"],
        //                //We need to deal with potential nullable fields:
        //                City = (rdrAuthor["City"] is DBNull) ? "" : (string)rdrAuthor["City"],
        //                State = (rdrAuthor["State"] is DBNull) ? "" : (string)rdrAuthor["state"],
        //                ZipCode = (rdrAuthor["ZipCode"] is DBNull) ? "" : (string)rdrAuthor["ZipCode"],
        //                Country = (rdrAuthor["Country"] is DBNull) ? "" : (string)rdrAuthor["Country"]

        //            }; //Initializing object syntax
        //        }
        //        rdrAuthor.Close();
        //        conn.Close();

        //    }
        //    return author;
        //}//Change to Get Products

        public void UpdateProduct(ProductModel product)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";
                conn.Open();
                SqlCommand cmdUpdateProduct = new SqlCommand(
                @"Update Products set
                ProductName = @ProductName,
                ProductDescription = @ProductDescription, 
                Price = @Price 
                Where ProductId = @ProductId",
                conn);
                cmdUpdateProduct.Parameters.AddWithValue("ProductId", product.ProductId);
                cmdUpdateProduct.Parameters.AddWithValue("ProductName", product.ProductName);
                cmdUpdateProduct.Parameters.AddWithValue("ProductDescription", product.ProductDescription);
                //nullable fields:
                //CITY
                if (product.Price != null)
                {
                    cmdUpdateProduct.Parameters.AddWithValue("Price", product.Price);
                }
                else
                {
                    cmdUpdateProduct.Parameters.AddWithValue("Price", DBNull.Value);
                }

                cmdUpdateProduct.ExecuteNonQuery();


                conn.Close();
            }
        }

        public void DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=uStore;Integrated Security=true";
                conn.Open();
                SqlCommand cmdDeleteProduct = new SqlCommand(
                    "Delete from Products where ProductId = @ProductId",
                    conn);
                cmdDeleteProduct.Parameters.AddWithValue("ProductId", id);
                cmdDeleteProduct.ExecuteNonQuery();
                conn.Close();
            }
        }



    }

}   

