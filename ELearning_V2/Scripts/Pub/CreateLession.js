var CreateLessionApp = angular.module("CreateLessionApp", []);

CreateLessionApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

CreateLessionApp.controller('CreateLessionController', function ($scope, $http, $window) {
    $scope.InitUserID = function (UserID) {
        $scope.UserID = UserID;
    }
    var formdata = new FormData();
    var file;
    $scope.getTheFiles = function ($files) {
        console.log($files);
        angular.forEach($files, function (value, key) {
            console.log(key + ' ' + value.name);
            formdata.set(key, value);
        });
        file = $files[0];
        console.log(formdata);
    };
    $scope.save = function () {
        //var a = angular.element("#Content").val();
        var data = CKEDITOR.instances.Content.getData();
        console.log(data);
        var url = $scope.URL.replace("watch?v=", "embed/");
        var LessionDTO = {
            Name: $scope.Name,
            URL: url,
            Content: CKEDITOR.instances.Content.getData(),
            UserID: $scope.UserID
        };
        $http({
            method: 'POST',
            url: '/Lop/CreateLession',
            data: JSON.stringify(LessionDTO)
        }).then(function successCallback(response) {
            ID = response.data;
            console.log(ID);
            console.log('filename ' + file.name);
            var blob = file.slice(0, file.size, 'image/jpg');
            newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
            console.log('filename ' + newFile);
            formdata.set(0, newFile);
            console.log(formdata.get(0));
            $http({
                method: 'POST',
                url: '/api/API/UploadLessionImage',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            }).then(function successCallback(response) {
                alert(response.data);
                if (response.data == "OK") {
                    $window.location.href = '/Lop/LessionDetail/' + ID;
                }
            });
        });
    }
});
