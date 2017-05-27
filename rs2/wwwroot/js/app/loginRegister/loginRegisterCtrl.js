//app.controller('loginRegisterCtrl', ['$http', '$location', '$rootScope',
//    function loginRegisterCtrl($http, $location, $rootScope) {
//   "use strict";
//	
//   var vm = this;
//   
//    
//    $rootScope.isAdmin = false;
//    
//    vm.lg_submit = function () {
//        var activationLink = "http://localhost:5000/api/auth/login";
//        var data = {};
//        console.log("prvo ovde");
//        if(lg_username.value === 'admin@gmail.com' && lg_password.value === 'admin'){
//            $rootScope.isAdmin = true;
//        }
//
//        data.email = lg_username.value;
//        data.password = lg_password.value;
//        console.log("ovde");
//        
//
//        $http({ method: "post", url: activationLink, data: data }).success(function (response) {
//            console.log(response);
//
//        }).error(function (error) {
//            console.log(error);
//        });
//
//        $location.path('/home');
//    }
//
//    vm.rg_submit = function () {
//        var data = {};
//
//        data.username = reg_username.value;
//        data.email = reg_email.value;
//        data.password = reg_password.value;
//
//        var url = "http://localhost:5000/api/users";
//
//        $http({ method: "post", url: url, data: data }).success(function (response) {
//            console.log(response);
//
//        }).error(function (error) {
//            console.log(error);
//            });
//
//        console.log("bio ovde");
//    }
//    
//	// Options for Message
//	//----------------------------------------------
////  var options = {
////	  'btn-loading': '<i class="fa fa-spinner fa-pulse"></i>',
////	  'btn-success': '<i class="fa fa-check"></i>',
////	  'btn-error': '<i class="fa fa-remove"></i>',
////	  'msg-success': 'All Good! Redirecting...',
////	  'msg-error': 'Wrong login credentials!',
////	  'useAJAX': false,
////  };
////
////	// Login Form
////	//----------------------------------------------
////	// Validation
////  $("#login-form").validate({
////  	rules: {
////      lg_username: "required",
////  	  lg_password: "required",
////    },
////  	errorClass: "form-invalid"
////  });
////  
////	// Form Submission
////  $("#login-form").submit(function() {
////  	remove_loading($(this));
////		
////		if(options['useAJAX'] == true)
////		{
////			// Dummy AJAX request (Replace this with your AJAX code)
////		  // If you don't want to use AJAX, remove this
////  	 // dummy_submit_form($(this));
////		
////		  // Cancel the normal submission.
////		  // If you don't want to use AJAX, remove this
////  	 // return false;
////		}
////  });
////	
////	// Register Form
////	//----------------------------------------------
////	// Validation
////  $("#register-form").validate({
////  	rules: {
////      reg_username: "required",
////  	  reg_password: {
////  			required: true,
////  			minlength: 5
////  		},
////   		reg_password_confirm: {
////  			required: true,
////  			minlength: 5,
////  			equalTo: "#register-form [name=reg_password]"
////  		},
////  		reg_email: {
////  	    required: true,
////  			email: true
////  		},
////  		reg_agree: "required",
////    },
////	  errorClass: "form-invalid",
////	  errorPlacement: function( label, element ) {
////	    if( element.attr( "type" ) === "checkbox" || element.attr( "type" ) === "radio" ) {
////    		element.parent().append( label ); // this would append the label after all your checkboxes/labels (so the error-label will be the last element in <div class="controls"> )
////	    }
////			else {
////  	  	label.insertAfter( element ); // standard behaviour
////  	  }
////    }
////  });
////
////  // Form Submission
////  $("#register-form").submit(function() {
////  	remove_loading($(this));
////		
////		if(options['useAJAX'] == true)
////		{
////			// Dummy AJAX request (Replace this with your AJAX code)
////		  // If you don't want to use AJAX, remove this
////  	  //dummy_submit_form($(this));
////		
////		  // Cancel the normal submission.
////		  // If you don't want to use AJAX, remove this
////  	  //return false;
////		}
////  });
////
////
////	// Loading
////	//----------------------------------------------
////  function remove_loading($form)
////  {
////  	$form.find('[type=submit]').removeClass('error success');
////  	$form.find('.login-form-main-message').removeClass('show error success').html('');
////  }
////
////  function form_loading($form)
////  {
////    $form.find('[type=submit]').addClass('clicked').html(options['btn-loading']);
////  }
////  
////  function form_success($form)
////  {
////	  $form.find('[type=submit]').addClass('success').html(options['btn-success']);
////	  $form.find('.login-form-main-message').addClass('show success').html(options['msg-success']);
////  }
////
////  function form_failed($form)
////  {
////  	$form.find('[type=submit]').addClass('error').html(options['btn-error']);
////  	$form.find('.login-form-main-message').addClass('show error').html(options['msg-error']);
////  }
//
//	// Dummy Submit Form (Remove this)
//	//----------------------------------------------
//	// This is just a dummy form submission. You should use your AJAX function or remove this function if you are not using AJAX.
////  function dummy_submit_form($form)
////  {
////  	if($form.valid())
////  	{
////  		form_loading($form);
////  		
////  		setTimeout(function() {
////  			form_success($form);
////  		}, 2000);
////  	}
////  }
// 
//}]);
