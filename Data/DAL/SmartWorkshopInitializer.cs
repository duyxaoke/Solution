using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System;

using System.Collections.Generic;


using System.Reflection;

using Shared.Common;


namespace Data.DAL
{
    public class DatabaseInitializer : DbMigrationsConfiguration<DatabaseContext>
    {
        public DatabaseInitializer()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(DatabaseContext context)
        {

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {

                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);

                var role = new ApplicationRole { Name = "Administrator" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "User" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "Manager" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Anonymous"))
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole { Name = "Anonymous" };
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = AccountIds.Admin.GetGuid().ToString(),
                    UserName = "admin", Name = "Tran Duy", Email = "duytran1402@gmail.com", Balance = 0, EmailConfirmed = true };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "Administrator");
            }
            if (!context.Users.Any(u => u.UserName == "user"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Id = AccountIds.User1.GetGuid().ToString(),
                    UserName = "user",
                    Name = "user",
                    Email = "user@gmail.com",
                    Balance = 0,
                    EmailConfirmed = true
                };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "User");
            }

            //TestDataSeed(context).GetAwaiter().GetResult();

            base.Seed(context);
        }

        #region Ids

        #region AccountIds
        enum AccountIds
        {
            [EnumGuid("240de0d4-ade7-417e-a034-9b63cc2de853")] Admin,
            [EnumGuid("1966c895-c0d4-40d6-b201-47c0dd0228e1")] User1
        }
        #endregion

        #region BillingAddressIds
        enum BillingAddressIds
        {
            [EnumGuid("975c62c9-5924-4b23-9e1b-d3de87118d42")] Billing0,
            [EnumGuid("6670f705-8a06-451c-b267-5dceb9c130b1")] Billing1,
            [EnumGuid("8ed5b69b-b4b2-452d-25e0-08d483e4de1d")] Billing2
        }
        #endregion

        #region OrderIds
        enum OrderIds
        {
            [EnumGuid("50ec3a54-0eab-4dfc-bad7-d1538f62f25e")] Order1,
            [EnumGuid("693721cb-61e5-41cd-9d99-376c08ab627b")] Order2
        }
        #endregion

        #region ProductIds
        enum ProductIds
        {
            [EnumGuid("337acae3-7adf-4372-8619-1cc9345c61ea")] RogG7,
            [EnumGuid("c85f8f8b-3245-4be5-9fa7-96f1df2dbdc7")] AcerPredatorGx,
            [EnumGuid("9de9aad6-7dca-4861-842f-20021a2c5fa0")] AsusGtx1080tiFounder,
            [EnumGuid("d9122044-3401-4bee-aaac-9c7802a7027e")] AsusGtx1070Strix,
            [EnumGuid("5f1c200c-b551-4ceb-9273-3ccf9c4718da")] RogStrixRx480O8G,
            [EnumGuid("6176109c-2219-4013-a3f3-7cf90b60d8be")] IntelCorei77700K,
            [EnumGuid("4701684d-f990-4829-8d5a-3d468155520f")] RazerBladeGTX1060,
            [EnumGuid("62e79149-f655-4bb4-ab81-934690c80264")] AmdRyzen71800X,
            [EnumGuid("5051b175-0b4c-4596-9b3d-7f47db3aa487")] LogitechG502,
        }
        #endregion

        #region CategoryIds
        enum CategoryIds
        {
            [EnumGuid("8c4825ef-8c4c-4162-b2e3-08d46c337976")] Laptop,
            [EnumGuid("572a88b0-17ef-4e6c-a806-ca35ad57a41b")] Mouse,
            [EnumGuid("21e88188-057e-41e0-8746-57ec2fd76a51")] Processors,
            [EnumGuid("fdc32bdd-013d-4ced-9106-c3e722e4650a")] VideoCard
        }
        #endregion

        #region ImageIds
        enum ImageIds
        {
            [EnumGuid("1c34435f-2dc2-45fc-a903-7bca40eb5674")] RogG7Front,
            [EnumGuid("cb7d5d64-283c-4d45-9c45-d48c9763956c")] RogG7Back,
            [EnumGuid("dd733338-513d-4e30-9e7f-d4b09f975dd3")] Predator17XFront,
            [EnumGuid("f27092f1-2931-4dd5-ab7f-3ef2126c9cf8")] Predator17XBack,
            [EnumGuid("af740663-4919-47dd-b2a5-a393af28bbd5")] AsusGtx1080tiFounder,
            [EnumGuid("4ae5aa7a-b7b7-4b47-94d9-180876233776")] AsusGtx1080tiFounder2,
            [EnumGuid("04096de0-531e-4f9d-848e-a2c36794181e")] AsusGgtx1070Strix,
            [EnumGuid("2f077ad1-ab0c-4ff0-864c-4b45e4c31d8c")] RogStrixRx480O8G,
            [EnumGuid("e09f3dd2-a176-47b3-8518-2015eaef32cc")] IntelCorei77700K,
            [EnumGuid("06cf5fcf-be1f-4690-a9ae-69dc4c35bca7")] TheRazerBladeFront,
            [EnumGuid("4e6bbd99-d7a4-470d-bc47-2a6e39389e0a")] TheRazerBladeSlide,
            [EnumGuid("6449aee5-0618-41f5-9c81-dba6ba41870c")] Ryzen71800x,
            [EnumGuid("9b1cd692-8d27-4661-a171-debe279a8961")] LogitechG502Main,
            [EnumGuid("baabfd5a-7851-49b6-b67b-046877de431c")] LogitechG502Side,
            [EnumGuid("139e1fb7-6f21-4d2c-8987-26091744fa4c")] LogitechG502Bottom
        }
        #endregion

        #region ManufacturerIds
        enum ManufacturerIds
        {
            [EnumGuid("609483bf-c285-4d67-92f3-08d46c31e55a")] Acer,
            [EnumGuid("c2c32a94-3a51-48be-9d1a-8a9a687bcb60")] AMD,
            [EnumGuid("8d942bc6-7407-417f-92f2-08d46c31e55a")] Asus,
            [EnumGuid("d96116b4-d107-4db2-bdbc-be493989d557")] Intel,
            [EnumGuid("b54c872d-a32b-4f8a-8ae1-12b61167cecd")] Logitech,
            [EnumGuid("0c69ba36-beb1-4054-b492-f361c836acc3")] Razer
        }
        #endregion

        #region SpecificationIds
        enum SpecificationIds
        {
            [EnumGuid("18f698d1-7060-485e-b411-7949491f54db")] NumberOfCores,
            [EnumGuid("af498fc2-aa53-4a36-b535-891428a92a84")] NumberOfThreads,
            [EnumGuid("473dd9cf-ec4d-452e-9e8a-88d9670cc75b")] Bit64Support,
            [EnumGuid("58f74c41-14b7-426f-aa46-ddbd73989292")] Accessories,
            [EnumGuid("d957dae6-c254-4266-b45c-27fa04f00761")] Battery,
            [EnumGuid("8d7163a1-ef0b-442b-88ce-55363cd9ddbc")] Brand,
            [EnumGuid("f8affde9-7566-448e-ba9f-d06de0fcd1b8")] Buttons,
            [EnumGuid("679ee965-7868-4812-8dad-b6f53e542ebd")] Color,
            [EnumGuid("c6a04689-3a3d-4346-b233-a9078ee57f0d")] CoolingDevice,
            [EnumGuid("739b9689-b4e5-4c30-8108-8cec7419aba9")] CoreName,
            [EnumGuid("f48d837a-22e1-43e1-967a-a989d5889f37")] Chipset,
            [EnumGuid("b9fe1b06-213a-4128-9329-250da2906852")] CPUSocketType,
            [EnumGuid("e229d9c0-459c-44f9-8f7d-d670cfb4d1d6")] CUDACore,
            [EnumGuid("19dfc537-f02a-4c7d-9919-5b939d08186f")] Dimensions,
            [EnumGuid("6144e4ab-722e-4b13-bffe-6f0ea8b168b2")] Display,
            [EnumGuid("6dcd49e6-7aa5-4971-80d3-30be12898633")] EngineClock,
            [EnumGuid("1cf7a9ef-11d9-4867-9c7d-ae7d7c46ba6c")] Features,
            [EnumGuid("42f0a5df-e976-4ab6-adab-e260f9cef244")] GamingSeries,
            [EnumGuid("27201fbe-59d6-42a4-b698-a75dcb3e9f52")] Graphic,
            [EnumGuid("a0e252f2-39df-4f19-a139-260dd2935097")] GraphicsEngine,
            [EnumGuid("4f7d03ab-c9a1-489c-9736-7ce7a49f89de")] HandOrientation,
            [EnumGuid("79136369-aadd-47e2-ac71-2511933881e5")] HyperThreadingSupport,
            [EnumGuid("eca1dc44-190e-4806-ba8a-1af16fbd8d24")] Interface,
            [EnumGuid("c2a6ac96-7de8-4bdc-a322-fc56f27c8fc8")] Keyboard,
            [EnumGuid("282b9279-919d-4300-8109-eb568c02e839")] L2Cache,
            [EnumGuid("f1c8f4aa-9693-4cda-b6e5-c9e967439577")] L3Cache,
            [EnumGuid("1af15e3b-f60d-4d7d-a562-f7dff9a99f20")] ManufacturingTech,
            [EnumGuid("26eb8a9b-3b09-4f47-889c-28859a35b777")] MaxTurboFrequency,
            [EnumGuid("f4b46786-9f8b-4b04-934c-8c43d270e9e1")] MaximumDpi,
            [EnumGuid("928a7270-7d70-4a37-9440-c650c2a6d782")] Memory,
            [EnumGuid("20dabff6-aea8-4197-88a6-a3de73d9c36c")] MemoryClock,
            [EnumGuid("d969d702-d7da-4eac-b203-2450c576bde7")] MemoryInterface,
            [EnumGuid("3dd9d028-1c7e-4b48-81d9-d50e6b8ce900")] Model,
            [EnumGuid("886a0409-5f0e-4152-a57a-8917f3a7a43b")] MouseAdjustableWeight,
            [EnumGuid("70f098c7-2d41-4c3a-b0fd-60dad9afb5a2")] MouseGripStyle,
            [EnumGuid("fc3dcda5-f9f3-4b2f-b038-f42bc5fa6774")] Name,
            [EnumGuid("f7af0f50-137c-4ce6-b27d-920c83d4ebc7")] Networking,
            [EnumGuid("ed46ee55-ac40-4d77-80be-69ab6b0d010c")] Operating,
            [EnumGuid("a5f9adab-4415-415c-8bcc-dac939acfa2f")] OperatingFrequency,
            [EnumGuid("753f0fb5-4cef-474e-ab93-b9b8797dc407")] OperatingSystemSupported,
            [EnumGuid("1ecc8164-69d8-4134-96a3-8ac89618be75")] PowerConnectors,
            [EnumGuid("75477c08-8245-4211-ab74-9c7c14d4dae9")] Processor,
            [EnumGuid("d073700d-c17e-41c8-a283-9abfeb8a8f6a")] ProcessorsType,
            [EnumGuid("9ed7299e-1637-4809-a375-5d0bdff8b613")] Resolution,
            [EnumGuid("0030dbcc-041e-41d5-977c-5ecdd4ceb6c3")] ScrollingCapability,
            [EnumGuid("2ac500ca-e15a-4785-9654-3eef7f26fb1c")] Series,
            [EnumGuid("1c9cef2e-b9df-4c90-b88e-f45f7d688646")] Software,
            [EnumGuid("8ad5f582-c787-4ba2-a49e-d8f8dd2ee621")] Storage,
            [EnumGuid("d5f23363-8404-4d34-8ef8-3a93babb8ad4")] SystemRequirement,
            [EnumGuid("a74cfddc-9450-4a3d-922b-e7670c9b7924")] ThermalDesignPower,
            [EnumGuid("583c2465-5e50-4f07-b2a3-4e0ba77bd374")] TrackingMethod,
            [EnumGuid("f3c9d2a1-e05d-4837-9507-e08404098750")] Type,
            [EnumGuid("2db5ac64-a42d-4bad-8eba-d26ff4e7f727")] VideoMemory,
            [EnumGuid("a73e43a3-d3d8-4b76-b1e6-274fb682d0a5")] VirtualizationTechnologySupport,
            [EnumGuid("88bcd475-ceb8-4ae3-a385-c3ec07b787e7")] VR,
            [EnumGuid("e611379b-5c1f-4286-8a54-9c8c45a5697d")] WebCam,
            [EnumGuid("93d8b1f6-8a3d-41e5-b3d5-7513bd7f3b33")] Weight
        }
        #endregion

        #endregion


        #region Seed Admin


        #endregion

        #region Seed User


        #endregion

    }
    #region EnumGuid class
    #endregion

}
