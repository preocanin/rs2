(function () {

    angular
        .module('app')
        .controller('dataCtrl', dataCtrl);

    dataCtrl.$inject = ['RecordService', '$location', '$rootScope', 'toastr'];

    function dataCtrl(RecordService, $location, $rootScope, toastr) {
        "use strict";

        var vm = this;
        vm.unos = false;
        vm.prikaz = true;
        vm.podatak = {};
        vm.records = [];
        vm.offset = 1;

        var gridDiv = document.querySelector('#myGrid');
        var myColDefs = [
            { headerName: "Make", field: "make" },
            { headerName: "Model", field: "model" },
            { headerName: "Price", field: "price" }
        ];
        var myRowData = [
            { make: "Toyota", model: "Celica", price: 35000 },
            { make: "Ford", model: "Mondeo", price: 32000 },
            { make: "Porsche", model: "Boxter", price: 72000 }
        ];


        var gridOptions = {

            // PROPERTIES - object properties, myRowData and myColDefs are created somewhere in your application
            rowData: myRowData,
            columnDefs: myColDefs,

            // EVENTS - add event callback handlers
            onRowClicked: function (event) { console.log('a row was clicked'); },
            onColumnResized: function (event) { console.log('a column was resized'); },
            onGridReady: function(event) {
                //setup first yourColumnDefs and yourGridData

                //now use the api to set the columnDefs and rowData
                this.columnDefs = myColDefs;
                this.rowData = myRowData;
            }
        }

        new agGrid.Grid(gridDiv, gridOptions);

        vm.kadUnos = function () {
            vm.unos = true;
            vm.prikaz = false;
        };

        vm.kadPrikaz = function () {
            vm.unos = false;
            vm.prikaz = true;

            RecordService.GetAll(vm.offset - 1)
                .then(function (response) {
                    vm.count = response.count;
                    vm.records = response.records;


                    /*var columnDefs = [
                        { headerName: "ID", field: "recordId" },
                        { headerName: "Pre X", field: "bx" },
                        { headerName: "Pre Y", field: "by" },
                        { headerName: "Posle X", field: "ax" },
                        { headerName: "Posle Y", field: "ay" },
                    ];

                    var rowData = [{ recordId: 1, bx: 2, by: 4, ax: 4, ay: 5 }];

                    vm.gridOptions = {
                        columnDefs: columnDefs,
                        rowData: rowData
                    };*/

                });

        };


        vm.kadPrikaz();

        vm.dodaj = function () {
            if (validan(vm.podatak)) {
                RecordService.AddRecord(vm.podatak)
                    .then(function (response) {
                        console.log(response);
                        toastr.success('Podatak je dodat');
                    });
            }
            else {
                toastr.error('Niste popunili sva polja');
            }
        };

        var validan = function (data) {

            if (data.Bx && data.By && data.Ax && data.Ay) {
                return true;
            }

            return false;
        };


        vm.obrisiSve = function () {
            RecordService.DeleteAllRecords()
                .then(function (response) {
                    console.log(response);
                });
        };

        vm.obrisiRed = function () {

        };

    }

})();