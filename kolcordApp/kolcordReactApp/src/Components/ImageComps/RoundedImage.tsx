type Props = {
  src: string
}

const RoundedImage: React.FC<Props> = ({src}) => {
  return (
    <div className='flex'>
      <div className='flex items-center'>
        <img src={src} alt="friend avatar" className='inline-block size-11 rounded-full object-cover border-2 border-black/30'/>
      </div>
    </div>
  );
};

export default RoundedImage;
