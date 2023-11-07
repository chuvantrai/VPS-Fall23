import { Button, Form, Input, Rate, Space } from 'antd';
import { useState } from 'react';
import TextArea from 'antd/es/input/TextArea.js';
import useFeedbackServices from '@/services/feedbackServices.js';

const desc = ['Rất tệ', 'Tệ', 'Bình thường', 'Tốt', 'Rất tốt'];

const FeedBackForm = ({ parkingZoneId }) => {
  const [form] = Form.useForm();
  const [valueRate, setValueRate] = useState(3);
  const feedBackService = useFeedbackServices();
  const onChangeRate = (value) => {
    setValueRate(value);
    form.setFieldValue('rate', value);
  };
  const onFinish = (values) => {
    feedBackService.createFeedBack(values, parkingZoneId);
  };

  return (
    <>
      <Form form={form} onFinish={onFinish}>
        <Form.Item
          label="Email của bạn"
          name="email"
          rules={[
            { required: true, message: 'Vui lòng nhập email của bạn' },
            {
              type: 'email',
              message: 'Email không hợp lệ',
            },
          ]}
        >
          <Input placeholder="Email"></Input>
        </Form.Item>
        <Form.Item label="Sao" name="rate">
          <span>
            <Rate tooltips={desc} onChange={onChangeRate} value={valueRate} />
            {valueRate ? <span className="ant-rate-text">{desc[valueRate - 1]}</span> : ''}
          </span>
        </Form.Item>
        <Form.Item
          label="Bình luận"
          name="comment"
          rules={[
            {
              max: 100,
              message: 'Bình luận tối đa 100 ký tự!',
            },
          ]}
        >
          <TextArea
            className={'max-h-[97px!important] min-h-[97px!important]'}
            rows={4}
            placeholder="Bình luận tối đa 100 ký tự!"
          />
        </Form.Item>
        <Form.Item className={'flex justify-center m-0'}>
          <Button className={'bg-[#1890FF] w-[200%]'} type="primary" htmlType={'submit'}>
            Đánh giá
          </Button>
        </Form.Item>
      </Form>
    </>
  );
};

export default FeedBackForm;
