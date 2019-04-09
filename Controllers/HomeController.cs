using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private DishContext dbContext;
        public HomeController(DishContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.ToList();
            
            return View(AllDishes);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult New(Dish NewDish)
        {
            if(ModelState.IsValid)
            {
                dbContext.Dishes.Add(NewDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult One(int id)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishID == id);
            return View(retrievedDish);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishID == id);
            return View(retrievedDish);
        }

        [HttpPost("update/{id}")]
        public IActionResult Edit(Dish updatedDish, int id)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishID == id);

            if(ModelState.IsValid)
            {
                retrievedDish.Name = updatedDish.Name;
                retrievedDish.Chef = updatedDish.Chef;
                retrievedDish.Calories = updatedDish.Calories;
                retrievedDish.Tastiness = updatedDish.Tastiness;
                retrievedDish.Description = updatedDish.Description;
                retrievedDish.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();

                return RedirectToAction("One", id);
            }
            else
            {
                return View(retrievedDish);
            }
        }
        [HttpGet("/delete/{id}")]
        public IActionResult Delete(int id)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishID == id);
            if(retrievedDish != null){
                dbContext.Remove(retrievedDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("One");
            }
            
        }
    }
}
