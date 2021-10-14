import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';
import { validFullName } from '../../regex.js';
import validator from 'validator'
import NumberFormat from 'react-number-format';
export class EmployeeEdit extends Component {
  static displayName = EmployeeEdit.name;

  constructor(props) {
    super(props);
    this.state = { id: 0, fullName: '', birthdate: '', tin: '', typeId: 1, salary: 0, loading: true, loadingSave: false };
    this.state.errors = { fullName: '', birthdate: '', tin: '', typeId: '', salary: '' };
  }

  componentDidMount() {
    this.getEmployee(this.props.match.params.id);
  }
  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
    this.setState({ errors: { [event.target.name]: '' } });
  }

  handleSubmit(e) {
    e.preventDefault();
     if (this.handleValidation()) {
      if (window.confirm("Are you sure you want to save?")) {
        this.saveEmployee();
       }
    }
  }

  handleValidation() {
    let fields = this.state;
    let error = this.state.errors;
    let formIsValid = true;
    //Name
    if (fields.fullName == '') {
      error.fullName = "This field is required";
      formIsValid = false;
    } else if (typeof fields.fullName !== "undefined") {
      if (!validFullName.test(fields.fullName)) {
        error.fullName = "This field accepts Letters and the following characters only (.' ,)";
        formIsValid = false;
      } else if (fields.fullName.length < 5) {
        error.fullName = "Please enter you full name";
        formIsValid = false;
      }
    }

    //Birthdate
    var today = new Date();
    var birthdate = new Date(fields.birthdate);
    if (fields.birthdate == '') {
      error.birthdate = "This field is required";
      formIsValid = false;
    } else if (!validator.isDate(fields.birthdate)) {
      error.birthdate = "Please enter valid date";
      formIsValid = false;
    } else if (birthdate > today) {
      error.birthdate = "Please enter valid birthdate";
      formIsValid = false;
    } else if (birthdate < today) {
      var age_now = today.getFullYear() - birthdate.getFullYear();
      var m = today.getMonth() - birthdate.getMonth();
      if (m < 0 || (m === 0 && today.getDate() < birthdate.getDate())) {
        age_now--;
      }
      if(age_now<18){
        error.birthdate = "Should be 18 years old and above";
        formIsValid = false;
      }
     
    }

    //Salary
    if (fields.salary == '') {
      error.salary = "This field is required";
      formIsValid = false;
    } else if (fields.salary == 0) {
      error.salary = "Please input salary greater than zero";
      formIsValid = false;
    }

    //TIN
    if (fields.tin == '') {
      error.tin = "This field is required";
      formIsValid = false;
    } else if (fields.tin.length < 9 || fields.tin.length > 12) {
      error.tin = "Please enter valid TIN. This should be 9-12 Digits.";
      formIsValid = false;
    }

    this.setState({ errors: error });
    return formIsValid;
  }

  handleValueChange(value) {
    this.state.salary = value;
  }
  render() {

    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <div>
        <form>
          <div className='form-row'>
            <div className='form-group col-md-6'>
              <label htmlFor='inputFullName4'>Full Name: *</label>
              <input type='text' className='form-control' id='inputFullName4' onChange={this.handleChange.bind(this)} name="fullName" value={this.state.fullName} placeholder='Last Name, First Name, Middle Name' />
            </div>
            <div className='form-group col-md-6'>
              <label htmlFor='inputBirthdate4'>Birthdate: *</label>
              <input type='date' className='form-control' id='inputBirthdate4' onChange={this.handleChange.bind(this)} name="birthdate" value={this.state.birthdate} placeholder='Birthdate' />
            </div>
          </div>
          <div className="form-row">
            <div className='form-group col-md-6'>
              <label htmlFor='inputTin4'>TIN: *</label>
              <input type='text' className='form-control' id='inputTin4' onChange={this.handleChange.bind(this)} value={this.state.tin} name="tin" placeholder='TIN' />
            </div>
            <div className='form-group col-md-6'>
              <label htmlFor='inputEmployeeType4'>Employee Type: *</label>
              <select id='inputEmployeeType4' onChange={this.handleChange.bind(this)} value={this.state.typeId} name="typeId" className='form-control'>
                <option value='1'>Regular</option>
                <option value='2'>Contractual</option>
              </select>
            </div>
          </div>
          <div className="form-row">
            <div className='form-group col-md-6'>
              {this.state.typeId === 1 ? <label htmlFor='inputSalary4'>Monthly Rate: *</label> : <label htmlFor='inputSalary4'>Daily Rate: *</label>}
              {/* <input type='text' className='form-control' id='inputSalary4' onChange={this.handleChange.bind(this)} value={this.state.salary} name="salary" placeholder={this.state.typeId === 1 ? 'Monthly Rate' : 'Daily Rate'} /> */}
              <NumberFormat className='form-control' thousandSeparator={true} decimalScale="2" fixedDecimalScale="true" allowNegative="false"
                value={this.state.salary} name="salary"
                isNumericString="true"
                onValueChange={(values) => {
                  this.handleValueChange(values.floatValue);
                }}
              />
            </div>
          </div>
          <button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingSave} className="btn btn-primary mr-2">{this.state.loadingSave ? "Loading..." : "Save"}</button>
          <button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
        </form>
      </div>;


    return (
      <div>
        <h1 id="tabelLabel" >Employee Edit</h1>
        <p>All fields are required</p>
        {contents}
      </div>
    );
  }

  async saveEmployee() {
    this.setState({ loadingSave: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
      method: 'PUT',
      headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
      body: JSON.stringify(this.state)
    };
    const response = await fetch('api/employees/' + this.state.id, requestOptions);

    if (response.status === 200) {
      this.setState({ loadingSave: false });
      alert("Employee successfully saved");
      this.props.history.push("/employees/index");
    }
    else {
      alert("There was an error occured.");
    }
  }

  async getEmployee(id) {
    this.setState({ loading: true, loadingSave: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const result = await response.json();
    this.setState({ id: result.data.id, fullName: result.data.fullName, birthdate: result.data.birthdate, tin: result.data.tin, typeId: result.data.typeId, salary: parseFloat(result.data.salary).toFixed(2), loading: false, loadingSave: false });
  }
}
