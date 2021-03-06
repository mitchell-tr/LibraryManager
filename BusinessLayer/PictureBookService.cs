using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Models;
using Models;
using AutoMapper;
using System.Collections;

namespace BusinessLayer
{
    public class PictureBookService : BaseService, IPictureBookService
    {

        public PictureBookService(ITransaction unitOfWork) : base(unitOfWork)
        {
        }
        public PictureBookModel Get(int id)
        {
            //Get the book
            var book = UnitOfWork.Books.Get(id);

            //Create the configuration that thells AutoMapper the classes to map
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Library, LibraryModel>();
                cfg.CreateMap<Customer, CustomerModel>();
                cfg.CreateMap<Reservation, ReservationModel>();
                cfg.CreateMap<Magazine, MagazineBookModel>();
                cfg.CreateMap<Novel, NovelBookModel>();
                cfg.CreateMap<PictureBook, PictureBookModel>();
                cfg.CreateMap<Magazine, IBookModel>().As<MagazineBookModel>();
                cfg.CreateMap<PictureBook, IBookModel>().As<PictureBookModel>();
                cfg.CreateMap<Novel, IBookModel>().As<NovelBookModel>();
            });

            //Create the mapper
            IMapper mapper = new Mapper(config);

            //Create the model to map the book entity to
            PictureBookModel model = new PictureBookModel();


            mapper.Map(book, model);

            //Do the mapping


            return model;
        }
        public int Add(PictureBookModel book)
        {
            Validate(book);

            //Ignore the as we are just adding the book
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PictureBookModel, PictureBook>()
                                                 .ForMember(x => x.Library, opt => opt.Ignore()));

            IMapper mapper = new Mapper(config);

            var data = new PictureBook();

            mapper.Map(book, data);

            //Add the book to the repository and save
            UnitOfWork.Books.Add(data);
            UnitOfWork.Save();

            return data.ID;
        }

        public int Update(PictureBookModel model)
        {
            Validate(model);

            //Ignore the as we are just updating the book
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PictureBookModel, PictureBook>()
                                                 .ForMember(x => x.Library, opt => opt.Ignore()));

            IMapper mapper = new Mapper(config);

            //Retrive the book to update
            var data = UnitOfWork.Books.Get(model.ID);

            //Map the model to the enitity
            mapper.Map(model, data);

            //Update and save the 
            UnitOfWork.Books.Update(data);
            UnitOfWork.Save();

            return data.ID;
        }
        public void Delete(int id)
        {
            UnitOfWork.Books.Delete(id);
            UnitOfWork.Save();
        }
        public IList<PictureBookModel> List()
        {
            var books = UnitOfWork.Books.List<PictureBook>();

            //Configure the mappings for all the classes AutoMapper may come across.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Library, LibraryModel>();
                cfg.CreateMap<Customer, CustomerModel>();
                cfg.CreateMap<Reservation, ReservationModel>();
                cfg.CreateMap<Magazine, MagazineBookModel>();
                cfg.CreateMap<Novel, NovelBookModel>();
                cfg.CreateMap<PictureBook, PictureBookModel>();
                cfg.CreateMap<Magazine, IBookModel>().As<MagazineBookModel>();
                cfg.CreateMap<PictureBook, IBookModel>().As<PictureBookModel>();
                cfg.CreateMap<Novel, IBookModel>().As<NovelBookModel>();
            });

            IMapper mapper = new Mapper(config);

            //Create the model list
            var models = new List<PictureBookModel>();

            //AutoMapper maps the classes and added the models to the list
            mapper.Map(books, models);

            return models;
        }
        public IEnumerable ListNames()
        {
            //ListNames return an anonymous class so doesn't need to be mapped
            return UnitOfWork.Books.List<PictureBook>();
        }
    }
}
