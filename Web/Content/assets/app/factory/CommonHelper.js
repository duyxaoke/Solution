var CommonHelper = function ($rootScope, $localstorage, $timeout, $q, $http) {
    let urlApi = "http://localhost:51581/api/";
    let service = {};

    service.ConfigUrl = urlApi + "Configs/";
    service.MenuUrl = urlApi + "Menus/";
    service.CurrencyUrl = urlApi +  "Currencies/";
    service.TransactionUrl = urlApi +  "Transactions/";
    service.RoleUrl = urlApi + "Roles/";
    service.UserUrl = urlApi + "Users/";
    service.RoomUrl = urlApi + "Rooms/";
    service.BetUrl = urlApi + "Bets/";
    service.TransactionUrl = urlApi + "Transactions/";

    service.DepWithType = {};
    service.DepWithType.Deposit = 0;
    service.DepWithType.Withdraw = 1;

    service.StatusTransaction = {};
    service.StatusTransaction.Pending = 0;
    service.StatusTransaction.Confirmed = 1;
    service.StatusTransaction.Cancel = 2;

    return service;
}
CommonHelper.$inject = ["$rootScope", "$localstorage", "$timeout", "$q", "$http"];
