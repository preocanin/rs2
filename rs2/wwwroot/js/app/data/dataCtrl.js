(function () {

    angular
        .module('app')
        .controller('dataCtrl', dataCtrl);

    dataCtrl.$inject = ['RecordService', '$location', '$rootScope', 'toastr', '$scope'];

    function dataCtrl(RecordService, $location, $rootScope, toastr, $scope) {
        "use strict";

        var vm = this;
        vm.unos = false;
        vm.prikaz = true;
        vm.podatak = {};
        vm.records = [];
        vm.offset = 1;

        var columnDefs = [
            { headerName: "ID", field: "recordId" },
            { headerName: "Pre X", field: "bx" },
            { headerName: "Pre Y", field: "by" },
            { headerName: "Posle X", field: "ax" },
            { headerName: "Posle Y", field: "ay" },
        ];

        vm.gridOptions = {
            enableColResize: true,
            debug: true,
            rowSelection: 'multiple',
            rowDeselection: true,
            columnDefs: columnDefs,
            // tell grid we want virtual row model type
            rowModelType: 'infinite',
            // how big each page in our page cache will be, default is 100
            paginationPageSize: 10,
            // how many extra blank rows to display to the user at the end of the dataset,
            // which sets the vertical scroll and then allows the grid to request viewing more rows of data.
            // default is 1, ie show 1 row.
            cacheOverflowSize: 2,
            // how many server side requests to send at a time. if user is scrolling lots, then the requests
            // are throttled downs
            // how many rows to initially show in the grid. having 1 shows a blank row, so it looks like
            // the grid is loading from the users perspective (as we have a spinner in the first col)
            infiniteInitialRowCount: 1,
            // how many pages to store in cache. default is undefined, which allows an infinite sized cache,
            // pages are never purged. this should be set for large data to stop your browser from getting
            // full of data
            maxBlocksInCache: 2,
            // PROPERTIES - object properties, myRowData and myColDefs are created somewhere in your application
            onGridReady: function () {
                vm.kadPrikaz();
            }
        }

        vm.kadUnos = function () {
            vm.unos = true;
            vm.prikaz = false;
        };

        function setRowData(allOfTheData) {
            var dataSource = {
                rowCount: null, // behave as infinite scroll
                getRows: function (params) {
                    console.log('asking for ' + params.startRow + ' to ' + params.endRow);
                    // At this point in your code, you would call the server, using $http if in AngularJS 1.x.
                    // To make the demo look real, wait for 500ms before returning
                    RecordService.GetAll(params.startRow, 10)
                        .then(function (response) {
                            vm.count = response.count;
                            vm.records = response.records;

                            // vm.rowData = [{ recordId: 1, bx: 2, by: 4, ax: 4, ay: 5 }];

                            // take a slice of the total rows
                            var rowsThisPage = vm.records.slice(params.startRow, params.endRow);
                            // if on or after the last page, work out the last row.
                            var lastRow = -1;
                            if (vm.records.length <= params.endRow) {
                                lastRow = vm.records.length;
                            }
                            // call the success callback
                            params.successCallback(rowsThisPage, lastRow);
                        });
                }
            };

            vm.gridOptions.api.setDatasource(dataSource);
            vm.gridOptions.api.sizeColumnsToFit();
        }

        vm.kadPrikaz = function () {
            vm.unos = false;
            vm.prikaz = true;
            vm.offset = vm.gridOptions.api.paginationGetRowCount() ? vm.gridOptions.api.paginationGetRowCount() - 1 : 0;

            setRowData(vm.records);

        };

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