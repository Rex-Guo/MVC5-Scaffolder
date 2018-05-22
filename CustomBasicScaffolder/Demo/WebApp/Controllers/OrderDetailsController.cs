﻿// <copyright file="OrderDetailsController.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>4/8/2018 12:45:13 PM </date>
// <summary>
// Create By Custom MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<OrderDetail>, Repository<OrderDetail>>();
// container.RegisterType<IOrderDetailService, OrderDetailService>();
// </summary>
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Z.EntityFramework.Plus;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using TrackableEntities;

namespace WebApp.Controllers
{
    //[Authorize]
	public class OrderDetailsController : Controller
	{
		//private StoreContext db = new StoreContext();
		private readonly IOrderDetailService  _orderDetailService;
		private readonly IUnitOfWorkAsync _unitOfWork;
		public OrderDetailsController (IOrderDetailService  orderDetailService, IUnitOfWorkAsync unitOfWork)
		{
			_orderDetailService  = orderDetailService;
			_unitOfWork = unitOfWork;
		}
        		// GET: OrderDetails/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index()
		{
			 return View();
		}
		// Get :OrderDetails/PageList
		// For Index View Boostrap-Table load  data 
		[HttpGet]
				 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
											var orderdetails  = await _orderDetailService
						               .Query(new OrderDetailQuery().Withfilter(filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
										var datarows = orderdetails .Select(  n => new { OrderCustomer = (n.Order==null?"": n.Order.Customer) ,ProductName = (n.Product==null?"": n.Product.Name) , Id = n.Id , ProductId = n.ProductId , Qty = n.Qty , Price = n.Price , Amount = n.Amount , OrderId = n.OrderId }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
                 [HttpGet]
        public async Task<ActionResult> GetDataByProductId (int  productid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
            			    var orderdetails  = await _orderDetailService
						               .Query(new OrderDetailQuery().ByProductIdWithfilter(productid,filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
				            var datarows = orderdetails .Select(  n => new { OrderCustomer = (n.Order==null?"": n.Order.Customer) ,ProductName = (n.Product==null?"": n.Product.Name) , Id = n.Id , ProductId = n.ProductId , Qty = n.Qty , Price = n.Price , Amount = n.Amount , OrderId = n.OrderId }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
                [HttpGet]
        public async Task<ActionResult> GetDataByOrderId (int  orderid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
            			    var orderdetails  = await _orderDetailService
						               .Query(new OrderDetailQuery().ByOrderIdWithfilter(orderid,filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
				            var datarows = orderdetails .Select(  n => new { OrderCustomer = (n.Order==null?"": n.Order.Customer) ,ProductName = (n.Product==null?"": n.Product.Name) , Id = n.Id , ProductId = n.ProductId , Qty = n.Qty , Price = n.Price , Amount = n.Amount , OrderId = n.OrderId }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        		[HttpPost]
				public async Task<JsonResult> SaveData(OrderDetailChangeViewModel orderdetails)
		{
			if (orderdetails.updated != null)
			{
				foreach (var item in orderdetails.updated)
				{
					_orderDetailService.Update(item);
				}
			}
			if (orderdetails.deleted != null)
			{
				foreach (var item in orderdetails.deleted)
				{
					_orderDetailService.Delete(item);
				}
			}
			if (orderdetails.inserted != null)
			{
				foreach (var item in orderdetails.inserted)
				{
					_orderDetailService.Insert(item);
				}
			}
			await _unitOfWork.SaveChangesAsync();
			return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
		}
						        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetOrders(string q="")
		{
			var orderRepository = _unitOfWork.RepositoryAsync<Order>();
			var data = await orderRepository.Queryable().Where(n=>n.Customer.Contains(q)).ToListAsync();
			var rows = data.Select(n => new { Id = n.Id, Customer = n.Customer });
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
						        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetProducts(string q="")
		{
			var productRepository = _unitOfWork.RepositoryAsync<Product>();
			var data = await productRepository.Queryable().Where(n=>n.Name.Contains(q)).ToListAsync();
			var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								// GET: OrderDetails/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var  orderDetail = await _orderDetailService.FindAsync(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			return View(orderDetail);
		}
		// GET: OrderDetails/Create
        		public ActionResult Create()
				{
			var orderDetail = new OrderDetail();
			//set default value
			var orderRepository = _unitOfWork.RepositoryAsync<Order>();
		   			ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer");
		   			var productRepository = _unitOfWork.RepositoryAsync<Product>();
		   			ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
		   			return View(orderDetail);
		}
		// POST: OrderDetails/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Order,Product,Id,ProductId,Qty,Price,Amount,OrderId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] OrderDetail orderDetail)
		{
			if (ModelState.IsValid)
			{
			 				_orderDetailService.Insert(orderDetail);
		   				await _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a OrderDetail record");
				return RedirectToAction("Index");
			}
			else {
			 var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			 if (Request.IsAjaxRequest())
			 {
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			 }
			 DisplayErrorMessage(modelStateErrors);
			}
						var orderRepository = _unitOfWork.RepositoryAsync<Order>();
						ViewBag.OrderId = new SelectList(await orderRepository.Queryable().ToListAsync(), "Id", "Customer", orderDetail.OrderId);
									var productRepository = _unitOfWork.RepositoryAsync<Product>();
						ViewBag.ProductId = new SelectList(await productRepository.Queryable().ToListAsync(), "Id", "Name", orderDetail.ProductId);
									return View(orderDetail);
		}
        // GET: OrderDetails/PopupEdit/5
        //[OutputCache(Duration = 360, VaryByParam = "id")]
		public async Task<JsonResult> PopupEdit(int? id)
		{
			
			var orderDetail = await _orderDetailService.FindAsync(id);
			return Json(orderDetail,JsonRequestBehavior.AllowGet);
		}

		// GET: OrderDetails/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var orderDetail = await _orderDetailService.FindAsync(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			var orderRepository = _unitOfWork.RepositoryAsync<Order>();
			ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer", orderDetail.OrderId);
			var productRepository = _unitOfWork.RepositoryAsync<Product>();
			ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name", orderDetail.ProductId);
			return View(orderDetail);
		}
		// POST: OrderDetails/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Order,Product,Id,ProductId,Qty,Price,Amount,OrderId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] OrderDetail orderDetail)
		{
			if (ModelState.IsValid)
			{
				orderDetail.TrackingState = TrackingState.Modified;
								_orderDetailService.Update(orderDetail);
								await   _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a OrderDetail record");
				return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			if (Request.IsAjaxRequest())
			{
				return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			}
			DisplayErrorMessage(modelStateErrors);
			}
						var orderRepository = _unitOfWork.RepositoryAsync<Order>();
						ViewBag.OrderId = new SelectList( await orderRepository.Queryable().ToListAsync(), "Id", "Customer", orderDetail.OrderId);
									var productRepository = _unitOfWork.RepositoryAsync<Product>();
						ViewBag.ProductId = new SelectList( await productRepository.Queryable().ToListAsync(), "Id", "Name", orderDetail.ProductId);
									return View(orderDetail);
		}
		// GET: OrderDetails/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var orderDetail = await _orderDetailService.FindAsync(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			return View(orderDetail);
		}
		// POST: OrderDetails/Delete/5
		[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var orderDetail = await  _orderDetailService.FindAsync(id);
			 _orderDetailService.Delete(orderDetail);
			await _unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a OrderDetail record");
			return RedirectToAction("Index");
		}
       
 
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "orderdetails_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  _orderDetailService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText)
		{
			TempData["SuccessMessage"] = msgText;
		}
		private void DisplayErrorMessage(string msgText)
		{
			TempData["ErrorMessage"] = msgText;
		}
		 
	}
}
