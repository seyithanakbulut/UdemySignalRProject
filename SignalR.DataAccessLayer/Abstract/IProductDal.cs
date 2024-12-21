﻿using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IProductDal : IGenericDal<Product>
    {
        // Ürünleri kategorileri ile beraber getiren metot
        List<Product> GetProductsWithCategories();

        int ProductCount();
        int ProductCountByCategoryNameHamburger();
        int ProductCountByCategoryNameDrink();

        // Ortalama fiyat
        decimal ProductPriceAvg();

        string ProductNameByMaxPrice();
        string ProductNameByMinPrice();

        decimal ProductAvgPriceByHamburger();

        decimal ProductPriceBySteakBurger();

        decimal TotalPriceByDrinkCategory();
        decimal TotalPriceBySaladCategory();

        List<Product> GetLast9Products();
    }
}