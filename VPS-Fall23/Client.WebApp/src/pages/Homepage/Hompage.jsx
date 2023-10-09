// import classNames from 'classnames/bind';
// import style from './Homepage.module.scss'
import './Homepage.css'
import { Input, Button } from 'antd';


function Homepage() {
  return <div className="home-container w-full h-full m-auto bg-slate-500 block">
    <div className='search-container m-auto flex flex-col justify-center content-center'>
      <Input className='search-input' placeholder="Search parking zone" />
      <Button type="primary" className='btn'>Find near parking zone</Button>
    </div>
  </div>;
}

export default Homepage;
