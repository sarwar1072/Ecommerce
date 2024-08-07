﻿using Ecommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using Ecommerce.Web.Models.CoverModelFolder;
using Framework.Exceptions;
using Ecommerce.Web.Models.SellerModel;

namespace Ecommerce.Web.Controllers
{
    public class CoverController : AreaBaseController<CoverController>
    {
        public CoverController(ILifetimeScope scope,IUserAccessor userAccessor):base(scope, userAccessor)
        {
        }
        public IActionResult IndexCO()
        {
            var model = _scope.Resolve<CoverVM>();
            return View(model);
        }

        public IActionResult AddSeller()
        {
            var model = _scope.Resolve<CreateSellerModel>();
            return View(model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult AddSeller(CreateSellerModel model)
        {
            model.ResolveDependency(_scope);

            if (ModelState.IsValid)
            {
                try
                {
                    model.AddSeller();
                    model.Response = new ResponesModelTwo("created successfully", ResponseType.Success);
                    return View(model);
                }
                catch (DuplicationException2 ex)
                {
                    model.Response = new ResponesModelTwo(ex.Message, ResponseType.Failure);
                }
                catch (Exception ex)
                {
                    model.Response = new ResponesModelTwo("failed! ", ResponseType.Failure);
                }
            }
            return View(model);
        }


        public IActionResult Add()
        {
            var model = _scope.Resolve<CreateCover>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(CreateCover model)
        {
            model.ResolveDependency(_scope);
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add();
                    ViewResponse("Success", ResponseType.Success);
                    return RedirectToAction(nameof(Index));
                }
                catch (DuplicationException ex)
                {
                    ViewResponse("Duplicate", ResponseType.Duplicate);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewResponse("Failure", ResponseType.Failure);

                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model=_scope.Resolve<EditCover>();
            //var model = new EditCover();
            model.Load(id);
            return View(model); 
        }
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(EditCover editCover)
        {
            editCover.ResolveDependency(_scope);
            if(ModelState.IsValid)
            {
                try
                {
                    editCover.EditModel();
                    ViewResponse("Success", ResponseType.Success);
                    return RedirectToAction(nameof(Index));
                } 
                catch(DuplicationException ex)
                { 
                    ViewResponse("Duplicate",ResponseType.Duplicate);   
                    return RedirectToAction(nameof(Index)); 
                }
                catch(Exception ex)
                {
                    ViewResponse("Failure", ResponseType.Failure);
                }
            }
            return View(editCover);
        }
        public IActionResult GetCover()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<CoverVM>();
            var data = model.GetCover(tableModel);
            return Json(data);
        }

    }
}
