app.controller('clientsCtrl', ['$location', function clientsCtrl ($location) {
    "use strict";
    
    var vm = this;
    
    vm.users = [
        {
            "id": 1,
            "username": 'mika'
        },
        {
            "id": 2,
            "username": 'peraMali'
        }
    ];
    
}]);