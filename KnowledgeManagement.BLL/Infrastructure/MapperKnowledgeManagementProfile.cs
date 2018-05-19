using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeManagement.BLL.Infrastructure
{

    public interface IMapperKnowledgeManagementProfile
    {
        void Init();
    }

    public class MapperKnowledgeManagementProfile : IMapperKnowledgeManagementProfile //,IMapper
    {

        public MapperKnowledgeManagementProfile()
        {
        }

        public void Init()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    //  cfg.CreateMap<MyClass, MyClass1>();
            //    //   cfg.CreateMap < Tuple<MyClass, MyClass1>,
            //    //  MyClass2>().ForMember(x=>x.c,x=>x.MapFrom(n=>n.Item1.a*n.Item2.b));
            //    //   cfg.CreateMap<DAL_EF.Good, GoodDTO>();
            //    cfg.CreateMap<DAL_EF.Good, GoodDTO>();
            //    cfg.CreateMap<GoodDTO, DAL_EF.Good>();
            //    cfg.CreateMap<DAL_EF.Category, CategoryDTO>();
            //    cfg.CreateMap<CategoryDTO, DAL_EF.Category>();
            //    //          
            //    // cfg.CreateMap<DAL_EF.Supplier, SupplierDTO>().ForMember(x => x.GoodsId,
            //    //     x => x.MapFrom(m => new List<Int64>() { 1, 2 }));

            //    //cfg.CreateMap<DAL_EF.Supplier, SupplierDTO>().ForMember(x => x.GoodsId,
            //    //       x => x.MapFrom(m =>new List<Int64>(m.Goods.Select(y=>y.Id).ToList())
            //    //           ));
            //    //cfg.CreateMap<DAL_EF.Good, Int64>().ForMember(x => x, x => x.MapFrom(m=>m.Id));
            //    cfg.CreateMap<DAL_EF.Supplier, SupplierDTO>().ForMember(x => x.GoodsId,
            //         x => x.MapFrom(m => (m.Goods.Select(y => y.Id))));

            //    cfg.CreateMap<SupplierDTO, DAL_EF.Supplier>();
            //    //todo
            //    //        cfg.CreateMap<DAL_EF.Supplier, Supplier2View>();
        }
    }
}
       

